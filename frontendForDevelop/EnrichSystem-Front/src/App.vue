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


//todo:等确定下来之后，换成真实请求
const tasks = ref<TaskItem[]>([])
const summaries = ref<SummaryItem>()

const ppTasks = computed(()=>{
  return tasks.value.filter(x=>x.type == 0)
})

const cpTasks = computed(()=>{
  return tasks.value.filter(x=> x.type == 1)
})

function completeTask(task:TaskItem)
{
  //todo:把获得的奖励填充进来
  alert("完成委托,获得奖励");
}

function postQuest(){
  alert("发布任务")
  //todo:发布任务也做出来
}

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
            <span>{{ task.title }}</span>
            <button class="complete-button" @click = "completeTask(task)">完成</button>
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
    <div class="post-quest">
      <button class="complete-button" @click = "postQuest">发布任务</button>
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
  display: flex;
  align-items: center;
  justify-content: space-between;
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
.post-quest{
  position: fixed;
  right: 15px;
  bottom: 75px;
  background-color: rgba(0, 0, 0, 0.85);
  color: #d8c089;
  padding: 12px 16px;
}

.complete-button {
  padding: 8px 16px;
  border: 2px solid #5a3b1d;
  border-radius: 4px;
  background: linear-gradient(to bottom, #8b5a2b, #6b3f1d);
  color: #f5e6c8;
  font-size: 14px;
  font-weight: bold;
  font-family: serif;
  cursor: pointer;
  box-shadow:
    0 3px 0 #3e2412,
    inset 0 1px 0 rgba(255, 240, 200, 0.25);
  text-shadow: 1px 1px 0 #3a2412;
  transition: all 0.15s ease;
}

.complete-button:hover {
  background: linear-gradient(to bottom, #9c6a38, #7a4a24);
  color: #fff3d6;
}

.complete-button:active {
  transform: translateY(2px);
  box-shadow:
    0 1px 0 #3e2412,
    inset 0 1px 0 rgba(255, 240, 200, 0.2);
}

.complete-button:disabled {
  background: linear-gradient(to bottom, #6e6254, #4f463d);
  border-color: #4a4036;
  color: #cbbda3;
  cursor: not-allowed;
  box-shadow: none;
  text-shadow: none;
  opacity: 0.8;
}

</style>
