<script setup lang="ts">

import { ref, computed, onMounted } from 'vue'
import type {SettlementResult,DailyRoutineRecordItem,DailyRoutineRecordGroup} from '@/types/DailyRoutineTypes'
import axios from 'axios'
import DailySettlementModal from '@/components/DailySettlementModal.vue'


interface TaskItem {
  id: number
  title: string
  type: number
}

interface SummaryItem {
  coppers: number
  platinum: number
}

// 每日结算
const showSettlementModal = ref(false)


const recentDailyRoutineRecords = ref<DailyRoutineRecordItem[]>([])
// 最近每日记录：前端按日期分组
const groupedDailyRoutineRecords = computed<DailyRoutineRecordGroup[]>(() => {
  const map = new Map<string, DailyRoutineRecordItem[]>()

  const sortedRecords = [...recentDailyRoutineRecords.value].sort((a, b) => {
    return normalizeDate(b.date).localeCompare(normalizeDate(a.date))
  })

  for (const record of sortedRecords) {
    const date = normalizeDate(record.date)

    if (!map.has(date)) {
      map.set(date, [])
    }

    map.get(date)!.push(record)
  }

  return Array.from(map.entries()).map(([date, records]) => ({
    date,
    records
  }))
})


function openSettlementModal() {
  showSettlementModal.value = true
}

function closeSettlementModal() {
  showSettlementModal.value = false
}

async function handleSettlementSubmitted(result: SettlementResult) {
  console.log('今日计算完成：', result)

  // 重新拉取余额
  await loadSummary()
}

async function loadRecentDailyRoutineRecords() {
  const response = await axios.get('http://localhost:5148/api/RoutineRecord/GetRecent?days=7')

  console.log('recent response:', response.data)

  const records = response.data.dailyRoutineRecords ?? []

  console.log('records:', records)

  recentDailyRoutineRecords.value = records.map((x: any) => ({
    id: x.id,
    dailyRoutineId: x.dailyRoutineId,
    name: x.name,
    date: x.date,
    isCompleted: x.isCompleted,
    amount: x.amount,
    currency: x.currency
  }))

  console.log('recentDailyRoutineRecords:', recentDailyRoutineRecords.value)
}
function normalizeDate(date: string) {
  // 后端如果返回 "2026-05-01T00:00:00"，这里截成 "2026-05-01"
  return date.substring(0, 10)
}
function getTodayString() {
  const now = new Date()
  const year = now.getFullYear()
  const month = String(now.getMonth() + 1).padStart(2, '0')
  const day = String(now.getDate()).padStart(2, '0')

  return `${year}-${month}-${day}`
}
function getYesterdayString() {
  const yesterday = new Date()
  yesterday.setDate(yesterday.getDate() - 1)

  const year = yesterday.getFullYear()
  const month = String(yesterday.getMonth() + 1).padStart(2, '0')
  const day = String(yesterday.getDate()).padStart(2, '0')

  return `${year}-${month}-${day}`
}
function formatDateTitle(date: string) {
  if (date === getTodayString()) {
    return '今天'
  }

  if (date === getYesterdayString()) {
    return '昨天'
  }

  return date
}


// todo:等确定下来之后，换成真实请求
const tasks = ref<TaskItem[]>([])
const summaries = ref<SummaryItem>()

const ppTasks = computed(() => {
  return tasks.value.filter(x => x.type == 0)
})

const cpTasks = computed(() => {
  return tasks.value.filter(x => x.type == 1)
})

function completeTask(task: TaskItem) {
  // todo:把获得的奖励填充进来
  alert('完成委托,获得奖励')
}

function postQuest() {
  alert('发布任务')
  // todo:发布任务也做出来
}

async function loadTasks() {
  const response = await axios.get('http://localhost:5148/api/Quest')
  console.log(response.data)

  tasks.value = response.data.map((x: any) => ({
    id: x.id,
    title: x.description,
    type: x.currencyType,
  }))
}

async function loadSummary() {
  console.log('summary start')
  const summaryRes = await axios.get('http://localhost:5148/api/Ledger/summary/1')
  console.log(summaryRes.data)
  summaries.value = summaryRes.data
}
function getCurrencyText(currency: number) {
  if (currency === 0) {
    return 'pp'
  }

  if (currency === 1) {
    return 'cp'
  }

  return ''
}
onMounted(async () => {
  try {
    await loadTasks()
    await loadSummary()
    await loadRecentDailyRoutineRecords()
  } catch (error) {
    // todo:描述换成dnd风格
    console.error('获取数据失败', error)
  }
})
</script>


<template>
  <div class="page">
    <div class="main-board">
      <div class="quest-board">
        <h2 class="board-title">任务看板</h2>

        <div class="quest-columns">
          <div class="task-column">
            <h3>pp任务</h3>

            <div v-for="task in ppTasks" :key="task.id" class="task-card">
              <span>{{ task.title }}</span>
              <button class="complete-button" @click="completeTask(task)">完成</button>
            </div>
          </div>

          <div class="task-column">
            <h3>cp任务</h3>

            <div v-for="task in cpTasks" :key="task.id" class="task-card">
              <span>{{ task.title }}</span>
              <button class="complete-button" @click="completeTask(task)">完成</button>
            </div>
          </div>
        </div>
      </div>

      <div class="records-panel">
        <h2 class="board-title">最近每日记录</h2>

        <div
          v-for="group in groupedDailyRoutineRecords"
          :key="group.date"
          class="record-group"
        >
          <h3 class="record-date">
            {{ group.date }}
          </h3>

          <div
            v-for="record in group.records"
            :key="record.id"
            class="record-card"
          >
            <div>
              <div class="record-name">
                {{ record.name }}
              </div>

              <div class="record-status">
                {{ record.isCompleted ? '已完成' : '未完成' }}
              </div>
            </div>

            <div
              class="record-amount"
              :class="{ 'record-amount-minus': record.amount < 0 }"
            >
              {{ record.amount > 0 ? '+' : '' }}{{ record.amount }}
              {{ record.currency === 0 ? 'pp' : 'cp' }}
            </div>
          </div>
        </div>

        <div v-if="groupedDailyRoutineRecords.length === 0" class="empty-records">
          还没有每日记录
        </div>
      </div>
    </div>

    <div class="right-bottom-panel">
      <button class="complete-button" @click="openSettlementModal">
        今日结算
      </button>

      <button class="complete-button" @click="postQuest">
        发布任务
      </button>

      <div class="summary-show">
        <div>pp: {{ summaries?.platinum ?? 0 }}</div>
        <div>cp: {{ summaries?.coppers ?? 0 }}</div>
      </div>
    </div>

    <DailySettlementModal
      v-if="showSettlementModal"
      @close="closeSettlementModal"
      @submitted="handleSettlementSubmitted"
    />
  </div>
</template>
<style scoped>
.page {
  min-height: 100vh;
  background-color: black;
  color: white;
  padding: 24px;
  padding-bottom: 180px;
}

/* 整个主内容区域：上下排列 */
.main-board {
  display: flex;
  flex-direction: column;
  gap: 24px;
  align-items: stretch;
}

/* =========================
   任务看板
   ========================= */

.quest-board {
  width: 100%;
  border: 1px solid #69215d;
  padding: 16px;
  background-color: rgba(20, 10, 25, 0.6);
  box-sizing: border-box;
}

.board-title {
  margin-top: 0;
  margin-bottom: 16px;
  color: #d8c089;
  font-family: serif;
}

/* pp任务和cp任务在任务看板内部左右排列 */
.quest-columns {
  display: flex;
  gap: 16px;
}

.task-column {
  flex: 1;
  border: 1px solid #69215d;
  padding: 16px;
  background-color: rgba(0, 0, 0, 0.35);
  box-sizing: border-box;
}

.task-column h3 {
  margin-top: 0;
  margin-bottom: 12px;
  color: #f5e6c8;
  font-family: serif;
}

.task-card {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 12px;

  border: 1px solid #999;
  padding: 8px;
  margin-bottom: 8px;

  background-color: rgba(0, 0, 0, 0.35);
}

/* =========================
   每日记录板块
   ========================= */

.records-panel {
  width: 60%;
  max-width: 760px;
  min-width: 420px;

  border: 1px solid #6f5a3a;
  padding: 16px;
  background-color: rgba(30, 20, 10, 0.6);
  box-sizing: border-box;

  /* 想靠左就用 flex-start */
  align-self: flex-start;

  /* 如果你想居中，把上面 align-self 删掉，改用下面两行 */
  /*
  align-self: center;
  */
}

.record-group {
  margin-bottom: 20px;
}

.record-date {
  margin-top: 0;
  margin-bottom: 8px;
  color: #d8c089;
  border-bottom: 1px solid #6f5a3a;
  padding-bottom: 4px;
  font-family: serif;
}

.record-card {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 12px;

  border: 1px solid #5a4a34;
  background-color: rgba(0, 0, 0, 0.45);
  padding: 8px;
  margin-bottom: 8px;
}

.record-name {
  font-weight: bold;
  color: #f5e6c8;
}

.record-status {
  margin-top: 4px;
  font-size: 12px;
  color: #aaa;
}

.record-amount {
  color: #9ee493;
  font-weight: bold;
  white-space: nowrap;
}

.record-amount-minus {
  color: #e49393;
}

.empty-records {
  color: #999;
  font-size: 14px;
}

/* =========================
   右下角固定操作区
   ========================= */

.right-bottom-panel {
  position: fixed;
  right: 24px;
  bottom: 24px;

  display: flex;
  flex-direction: column;
  align-items: flex-end;
  gap: 16px;

  z-index: 20;
}

.summary-show {
  border: 1px solid #6f5a3a;
  background-color: rgba(0, 0, 0, 0.85);
  color: #d8c089;
  padding: 12px 16px;
  min-width: 80px;
  box-sizing: border-box;
  font-family: serif;
}

/* =========================
   按钮
   ========================= */

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

/* =========================
   小屏幕适配
   ========================= */

@media (max-width: 900px) {
  .quest-columns {
    flex-direction: column;
  }

  .records-panel {
    width: 100%;
    max-width: none;
    min-width: 0;
  }

  .right-bottom-panel {
    right: 16px;
    bottom: 16px;
  }
}
</style>
