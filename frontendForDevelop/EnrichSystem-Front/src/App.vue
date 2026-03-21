<script setup lang="ts">
import HelloWorld from './components/HelloWorld.vue'
import TheWelcome from './components/TheWelcome.vue'
import { ref, computed, onMounted } from 'vue'
import axios from 'axios'
interface TaskItem {
  id: number
  title: string
  type: number
}

// const tasks = ref<TaskItem[]>([
//   { id: 1, title: '讨伐墓园里的骷髅兵', type: 'pp' },
//   { id: 2, title: '给酒馆老板送一桶麦酒', type: 'cp' },
//   { id: 3, title: '调查北边森林的狼嚎', type: 'pp' },
//   { id: 4, title: '替铁匠收集3块铁矿石', type: 'cp' },
//   { id: 5, title: '护送商人到西门集市', type: 'pp' }
// ])


//todo:等确定下来之后，换成真实请求
const tasks = ref<TaskItem[]>([])

const ppTasks = computed(()=>{
  return tasks.value.filter(x=>x.type == 0)
})

const cpTasks = computed(()=>{
  return tasks.value.filter(x=> x.type == 1)
})
onMounted(async()=>
{
  try
  {
    console.log('request start')
    //todo:把自己的接口写出来
    const response = await axios.get('http://localhost:5148/api/Quest')
    console.log(response.data)
    tasks.value = response.data.map(
      (x:any)=>
      {
        
      }
    )
  }
  catch(error){
    //todo:描述换成dnd风格
    console.error('获取任务失败',error)
  }
})
</script>

<template>
  <div class="page">
    <div class="board">
      <div class = "task-column">
        <h2>pp任务</h2>
        <div v-for="task in ppTasks":key="task.id">
                    {{ task.title }}
        </div>
      </div>
      <div class="task-column">
        <h2>cp任务</h2>
            <div v-for="task in cpTasks":key="task.id">         
               {{ task.title }}
            </div>
      </div>
    </div>
  </div>
</template>


<style scoped>
.page {
  min-height: 100vh;
  background-color: black;
  color: white;
  padding: 24px;
}

.board {
  display: flex;
  gap: 24px;
}

.task-column {
  flex: 1;
  border: 1px solid #666;
  padding: 16px;
}

.task-card {
  border: 1px solid #999;
  padding: 8px;
  margin-bottom: 8px;
}
</style>
