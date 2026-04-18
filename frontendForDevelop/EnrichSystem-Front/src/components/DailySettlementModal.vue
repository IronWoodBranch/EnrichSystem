<script setup lang="ts">
import { computed, onMounted, reactive, ref } from 'vue'
import type { SettlementResult, Question, GetDailyRoutineDto } from '@/types/DailyRoutineTypes'

/**
 * 后端返回的每日任务 DTO
 */


//获取所有的日常任务
const GET_DAILY_ROUTINE_URL = 'http://localhost:5148/api/DailyRoutine'
//完成任务的接口发送
const SUBMIT_DAILYROUTINE_URL = 'http://localhost:5148/api/DailyRoutine/complete'

const emit = defineEmits<{
  (e: 'close'): void
  (e: 'submitted', result: SettlementResult): void
}>()

const questions = ref<Question[]>([])

/**
 * 动态答案表
 * key: 任务key
 * value: true / false / null
 */
const answers = reactive<Record<string, boolean | null>>({})

const currentStep = ref(0)
const isLoading = ref(false)
const isSubmitting = ref(false)
const loadErrorMessage = ref('')
const settlementResult = ref<SettlementResult | null>(null)

/**
 * 当前题目
 */
const currentQuestion = computed(() => {
  return questions.value[currentStep.value] ?? null
})

/**
 * 是否最后一题
 */
const isLastStep = computed(() => {
  return questions.value.length > 0 && currentStep.value === questions.value.length - 1
})

/**
 * 当前题是否已作答
 */
const canGoNext = computed(() => {
  const question = currentQuestion.value
  if (!question) return false

  const value = answers[question.key]
  return value === true || value === false
})

function closeModal() {
  emit('close')
}

/**
 * 把后端 DTO 转成前端 Question
 */
function mapDtoToQuestion(dto: GetDailyRoutineDto): Question {
  return {
    id: dto.id,
    key: dto.key,
    label: dto.name,
    amount: dto.amount,
  }
}

/**
 * 根据任务列表初始化 answers
 */
function initializeAnswers(questionList: Question[]) {
  Object.keys(answers).forEach((key) => {
    delete answers[key]
  })

  questionList.forEach((question) => {
    answers[question.key] = null
  })
}

/**
 * 获取真实任务列表
 */
async function loadQuestions() {
  isLoading.value = true
  loadErrorMessage.value = ''

  try {
    const response = await fetch(GET_DAILY_ROUTINE_URL, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      },
    })

    if (!response.ok) {
      throw new Error(`获取每日任务失败，状态码：${response.status}`)
    }

    const data: GetDailyRoutineDto[] = await response.json()
    const mappedQuestions = data.map(mapDtoToQuestion)

    questions.value = mappedQuestions
    initializeAnswers(mappedQuestions)
    currentStep.value = 0

  } catch (error) {
    console.error(error)
    loadErrorMessage.value = '获取每日任务失败，请稍后重试'
    questions.value = []
  } finally {
    isLoading.value = false
  }
}

function selectBoolean(value: boolean) {
  const question = currentQuestion.value
  if (!question) return

  answers[question.key] = value

  if (!isLastStep.value) {
    currentStep.value++
  }
}

function goNext() {
  if (!canGoNext.value) return
  if (!isLastStep.value) {
    currentStep.value++
  }
}

function goPrev() {
  if (currentStep.value > 0) {
    currentStep.value--
  }
}


async function submitSettlement() {
  if (!canGoNext.value) return

  isSubmitting.value = true

  try {
    const payload = {
      dailyRoutines: questions.value.map((question) => ({
        id: question.id,
        isCompleted: answers[question.key] === true,
      })),
    }

    console.log('payload:', payload)

    const response = await fetch(SUBMIT_DAILYROUTINE_URL, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(payload),
    })

    if (!response.ok) {
      const errorText = await response.text()
      console.error('后端返回错误内容:', errorText)
      throw new Error(`提交失败，请稍后再试: ${response.status}`)
    }

    const result: SettlementResult = await response.json()

    settlementResult.value = result
    emit('submitted', result)
  } catch (error) {
    console.error('提交error：', error)
  } finally {
    isSubmitting.value = false
  }
}

function retryLoad() {
  loadQuestions()
}

onMounted(() => {
  loadQuestions()
  console.log('子组件onmounted执行完毕')
})
</script>

<template>
  <div class="overlay">
    <div class="modal">
      <!-- 加载中 -->
      <template v-if="isLoading">
        <div class="state-area">
          <h2>加载中...</h2>
          <p>正在获取每日任务</p>
        </div>
      </template>

      <!-- 加载失败 -->
      <template v-else-if="loadErrorMessage">
        <div class="state-area">
          <h2>加载失败</h2>
          <p>{{ loadErrorMessage }}</p>

          <div class="actions">
            <button @click="closeModal">关闭</button>
            <button @click="retryLoad">重试</button>
          </div>
        </div>
      </template>

      <!-- 没有任务 -->
      <template v-else-if="!settlementResult && questions.length === 0">
        <div class="state-area">
          <h2>没有可结算的任务</h2>
          <div class="actions">
            <button @click="closeModal">关闭</button>
          </div>
        </div>
      </template>

      <!-- 问答中 -->
      <template v-else-if="!settlementResult">
        <div class="modal-header">
          <h2>今日结算</h2>
          <p>{{ currentStep + 1 }} / {{ questions.length }}</p>
        </div>

        <div v-if="currentQuestion" class="question-area">
          <h3>{{ currentQuestion.label }}</h3>

          <div class="boolean-buttons">
            <button type="button" @click="selectBoolean(true)">是</button>
            <button type="button" @click="selectBoolean(false)">否</button>
          </div>

          <p class="current-value">
            当前选择：
            <span v-if="answers[currentQuestion.key] === true">是</span>
            <span v-else-if="answers[currentQuestion.key] === false">否</span>
            <span v-else>未选择</span>
          </p>
        </div>

        <div class="actions">
          <button type="button" @click="closeModal">取消</button>
          <button v-if="currentStep > 0" type="button" @click="goPrev">上一步</button>

          <button v-if="!isLastStep" type="button" :disabled="!canGoNext" @click="goNext">
            下一步
          </button>

          <button v-else type="button" :disabled="!canGoNext || isSubmitting" @click="submitSettlement">
            {{ isSubmitting ? '结算中...' : '确认结算' }}
          </button>
        </div>
      </template>

      <!-- 结算完成 -->
      <template v-else>
        <div class="result-area">
          <h2>结算完成</h2>
          <p>获得 PP：{{ settlementResult?.platinum ?? 0 }}</p>
          <p>获得 CP：{{ settlementResult?.copper ?? 0 }}</p>

          <div>
            <h3>本次明细</h3>

            <ul v-if="settlementResult?.routineDetails?.length">
              <li v-for="item in settlementResult.routineDetails" :key="item.id">
                {{ item.name }} -
                <span v-if="item.isCompleted">〇</span>
                <span v-else>×</span>
                /
                {{ item.amount }}
                <span v-if="item.currencyType === 1">PP</span>
                <span v-else-if="item.currencyType ===2">CP</span>
              </li>
            </ul>

            <p v-else>没有明细</p>
          </div>

          <div class="actions">
            <button type="button" @click="closeModal">关闭</button>
          </div>
        </div>
      </template>
    </div>
  </div>
</template>

<style scoped>
.overlay {
  position: fixed;
  inset: 0;
  background: rgba(33, 24, 17, 0.72);
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 16px;
}

.modal {
  width: 100%;
  max-width: 560px;
  color: #3a2817;
  background:
    linear-gradient(180deg, rgba(244, 231, 203, 0.98) 0%, rgba(229, 210, 176, 0.98) 100%);
  border: 3px solid #6b4a2b;
  border-radius: 14px;
  padding: 24px;
  box-sizing: border-box;
  box-shadow:
    0 16px 40px rgba(0, 0, 0, 0.45),
    inset 0 0 0 2px rgba(120, 84, 49, 0.22);
  position: relative;
}

.modal::before {
  content: "";
  position: absolute;
  inset: 10px;
  border: 1px solid rgba(107, 74, 43, 0.35);
  border-radius: 10px;
  pointer-events: none;
}

.modal-header {
  margin-bottom: 24px;
  border-bottom: 2px solid rgba(107, 74, 43, 0.25);
  padding-bottom: 14px;
}

.modal-header h2 {
  margin: 0 0 8px;
  font-size: 28px;
  line-height: 1.2;
  color: #4a2f1a;
  letter-spacing: 1px;
}

.modal-header p {
  margin: 0;
  color: #6a4c31;
  font-size: 14px;
}

.question-area {
  margin-bottom: 24px;
  padding: 18px 16px;
  background: rgba(255, 248, 231, 0.42);
  border: 1px solid rgba(107, 74, 43, 0.22);
  border-radius: 10px;
}

.question-area h3 {
  margin: 0 0 18px;
  font-size: 22px;
  line-height: 1.5;
  color: #4a2f1a;
  font-weight: 700;
}

.boolean-buttons {
  display: flex;
  gap: 14px;
  margin-bottom: 14px;
}

button {
  font: inherit;
}

.boolean-buttons button,
.actions button,
.result-area button,
.state-area button {
  min-width: 92px;
  height: 42px;
  padding: 0 18px;
  border-radius: 8px;
  cursor: pointer;
  border: 1px solid #5e4025;
  color: #f8ecd6;
  background:
    linear-gradient(180deg, #8b5e34 0%, #6f4928 100%);
  box-shadow:
    inset 0 1px 0 rgba(255, 240, 210, 0.2),
    0 2px 0 rgba(70, 43, 22, 0.85);
  transition: transform 0.08s ease, filter 0.12s ease;
}

.boolean-buttons button:hover,
.actions button:hover,
.result-area button:hover,
.state-area button:hover {
  filter: brightness(1.06);
}

.boolean-buttons button:active,
.actions button:active,
.result-area button:active,
.state-area button:active {
  transform: translateY(1px);
  box-shadow:
    inset 0 1px 0 rgba(255, 240, 210, 0.12),
    0 1px 0 rgba(70, 43, 22, 0.85);
}

.actions {
  display: flex;
  gap: 12px;
  flex-wrap: wrap;
  margin-top: 8px;
}

.actions button:disabled,
.boolean-buttons button:disabled,
.result-area button:disabled,
.state-area button:disabled {
  opacity: 0.55;
  cursor: not-allowed;
  filter: grayscale(0.15);
}

.current-value {
  color: #6a4c31;
  margin: 0;
  font-size: 14px;
}

.result-area,
.state-area {
  text-align: center;
  color: #4a2f1a;
  padding: 10px 0;
}

.result-area h2,
.state-area h2 {
  margin-top: 0;
  margin-bottom: 12px;
  font-size: 28px;
  color: #4a2f1a;
}

.result-area h3 {
  color: #5a3a20;
  margin-top: 20px;
  margin-bottom: 10px;
}

.result-area p,
.state-area p {
  color: #6a4c31;
  line-height: 1.7;
}

.result-area ul {
  text-align: left;
  display: inline-block;
  margin: 12px 0 24px;
  padding-left: 22px;
  color: #4a2f1a;
}

.result-area li {
  margin-bottom: 6px;
}

.state-area .actions,
.result-area .actions {
  justify-content: center;
}

@media (max-width: 640px) {
  .modal {
    padding: 18px;
    border-radius: 12px;
  }

  .modal-header h2,
  .result-area h2,
  .state-area h2 {
    font-size: 24px;
  }

  .question-area h3 {
    font-size: 19px;
  }

  .boolean-buttons {
    flex-direction: column;
  }

  .boolean-buttons button,
  .actions button,
  .result-area button,
  .state-area button {
    width: 100%;
  }
}
</style>