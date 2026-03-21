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
interface SummaryItem{
  coppers: number
  platinum: number
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
const summaries = ref<SummaryItem>()

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
      (x:any)=>( //在js里面，这个括号能包一个对象出去
      {
        id:x.id,
        title:x.description,
        type: x.currencyType
      }
    ))
    
    console.log('summary start')
    const summaryRes = await axios.get('http://localhost:5148/api/Ledger/summary/1')
    console.log(summaryRes.data)
    summaries.value = summaryRes.data
    
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
        <div v-for="task in ppTasks" :key="task.id" class ="task-card">
                    {{ task.title }}
        </div>
      </div>
      <div class="task-column">
        <h2>cp任务</h2>
            <div v-for="task in cpTasks" :key="task.id" class ="task-card">         
               {{ task.title }}
            </div>
      </div>
    <div class="summary-show">
        pp:{{ summaries?.platinum }}
        cp:{{ summaries?.coppers }}
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
  border: 1px solid #69215d;
  padding: 16px;
}

.task-card {
  border: 1px solid #999;
  padding: 8px;
  margin-bottom: 8px;
}
.summary-show {
  /* fixed不随页面移动，固定在某个地方，这就固定到了右下角 */
  position: fixed;
  right: 24px;
  bottom: 24px;
  border: 1px solid #6f5a3a;
  background-color: rgba(0, 0, 0, 0.85);
  color: #d8c089;
  padding: 12px 16px;
}
</style>
