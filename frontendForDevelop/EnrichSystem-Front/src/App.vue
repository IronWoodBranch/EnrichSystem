<script setup lang="ts">
import { ref, computed } from 'vue'

interface TaskItem {
  id: number
  title: string
  type: 'pp' | 'cp'
}

const tasks = ref<TaskItem[]>([
  { id: 1, title: '讨伐墓园里的骷髅兵', type: 'pp' },
  { id: 2, title: '给酒馆老板送一桶麦酒', type: 'cp' },
  { id: 3, title: '调查北边森林的狼嚎', type: 'pp' },
  { id: 4, title: '替铁匠收集3块铁矿石', type: 'cp' },
  { id: 5, title: '护送商人到西门集市', type: 'pp' }
])

const ppTasks = computed(() => {
  return tasks.value.filter(x => x.type === 'pp')
})

const cpTasks = computed(() => {
  return tasks.value.filter(x => x.type === 'cp')
})
</script>

<template>
  <div class="page">
    <div class="board">
      <div class="task-column">
        <h2>PP任务</h2>
        <div
          v-for="task in ppTasks"
          :key="task.id"
          class="task-card"
        >
          {{ task.title }}
        </div>
      </div>

      <div class="task-column">
        <h2>CP任务</h2>
        <div
          v-for="task in cpTasks"
          :key="task.id"
          class="task-card"
        >
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