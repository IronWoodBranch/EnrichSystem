<script setup lang="ts">
import HelloWorld from './components/HelloWorld.vue'
import TheWelcome from './components/TheWelcome.vue'
import { ref, computed, onMounted } from 'vue'
import axios from 'axios'
interface TaskItem {
  id: number
  title: string
  type: 'pp' | 'cp'
}
const tasks = ref<TaskItem[]>([])

const ppTasks = computed(()=>{
  return tasks.value.filter(x=>x.type == 'pp')
})

const cpTasks = computed(()=>{
  return tasks.value.filter(x=> x.type == 'cp')
})
onMounted(async()=>
{
  try
  {
    //todo:把自己的接口写出来
    const response = await axios.get('xxx/xxx')
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
        <div v-for="task in ppTasks":key="task.id"></div>
      </div>
    </div>
  </div>
</template>

<style scoped>
header {
  line-height: 1.5;
}

.logo {
  display: block;
  margin: 0 auto 2rem;
}

@media (min-width: 1024px) {
  header {
    display: flex;
    place-items: center;
    padding-right: calc(var(--section-gap) / 2);
  }

  .logo {
    margin: 0 2rem 0 0;
  }

  header .wrapper {
    display: flex;
    place-items: flex-start;
    flex-wrap: wrap;
  }
}
</style>
