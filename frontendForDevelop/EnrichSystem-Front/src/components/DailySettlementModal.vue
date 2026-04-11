<script setup lang="ts">
import { computed, reactive, ref } from 'vue'

type QuestionType = 'boolean' | 'time'

const answers = reactive({
  sleeptime: '',
  roomClean: null as boolean | null,
  japaneseStudy: null as boolean | null,
  running: null as boolean | null,
  laundry: null as boolean | null,
  rubbish: null as boolean | null,
})

type AnswerKey = keyof typeof answers

type Question = {
  key: AnswerKey
  label: string
  type: QuestionType
}

type SettlementResult = {
  earnedPlatinum: number
  earnedCopper: number
  completedItems: string[]
  sleepTime: string
}

const emit = defineEmits<{
  (e: 'close'): void
  (e: 'submitted', result: SettlementResult): void
}>()

function closeModal() {
  emit('close')
}

const questions: Question[] = [
  { key: 'sleeptime', label: '昨天几点睡', type: 'time' },
  { key: 'roomClean', label: '房间是否整洁？', type: 'boolean' },
  { key: 'japaneseStudy', label: '日语是否学习？', type: 'boolean' },
  { key: 'running', label: '是否跑步', type: 'boolean' },
  { key: 'laundry', label: '洗衣服', type: 'boolean' },
  { key: 'rubbish', label: '扔垃圾', type: 'boolean' },
]

const currentStep = ref(0)
const isSubmitting = ref(false)
const settlementResult = ref<SettlementResult | null>(null)

const currentQuestion = computed(() => questions[currentStep.value] ?? null)
const isLastStep = computed(() => currentStep.value === questions.length - 1)

function selectBoolean(value: boolean) {
  const question = currentQuestion.value
  if (!question) return
  if (question.type !== 'boolean') return
  answers[question.key] = value as never
  goNext()
}

function canGoNext() {
  const question = currentQuestion.value
  if (!question) return false

  const value = answers[question.key]

  if (question.type === 'boolean') {
    return value === true || value === false
  }

  if (question.type === 'time') {
    return typeof value === 'string' && value.trim() !== ''
  }

  return false
}

function goNext() {
  if (!canGoNext()) return
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
  if (!canGoNext()) return

  isSubmitting.value = true

  try {
    const fakeResponse: SettlementResult = {
      earnedPlatinum: 1,
      earnedCopper: 4,
      completedItems: [
        answers.japaneseStudy ? '学习日语' : null,
        answers.roomClean ? '房间整洁' : null,
        answers.running ? '跑步' : null,
        answers.laundry ? '洗衣服' : null,
        answers.rubbish ? '扔垃圾' : null,
      ].filter((item): item is string => item !== null),
      sleepTime: answers.sleeptime,
    }

    settlementResult.value = fakeResponse
    emit('submitted', fakeResponse)
  } finally {
    isSubmitting.value = false
  }
}
</script>

<template>
  <div class="overlay">
    <div class="modal">
      <template v-if="!settlementResult">
        <div class="modal-header">
          <h2>今日结算</h2>
          <p>{{ currentStep + 1 }} / {{ questions.length }}</p>
        </div>

        <div class="question-area" v-if="currentQuestion">
          <h3>{{ currentQuestion.label }}</h3>

          <template v-if="currentQuestion.type === 'boolean'">
            <div class="boolean-buttons">
              <button @click="selectBoolean(true)">是</button>
              <button @click="selectBoolean(false)">否</button>
            </div>

            <p class="current-value">
              当前选择：
              <span v-if="answers[currentQuestion.key] === true">是</span>
              <span v-else-if="answers[currentQuestion.key] === false">否</span>
              <span v-else>未选择</span>
            </p>
          </template>

          <template v-else-if="currentQuestion.type === 'time'">
            <input
              v-model="answers.sleeptime"
              type="time"
            />
          </template>
        </div>

        <div class="actions">
          <button @click="closeModal">取消</button>
          <button v-if="currentStep > 0" @click="goPrev">上一步</button>

          <button
            v-if="!isLastStep"
            :disabled="!canGoNext()"
            @click="goNext"
          >
            下一步
          </button>

          <button
            v-else
            :disabled="!canGoNext() || isSubmitting"
            @click="submitSettlement"
          >
            确认结算
          </button>
        </div>
      </template>

      <template v-else>
        <div class="result-area">
          <h2>结算完成</h2>
          <p>获得 PP：{{ settlementResult.earnedPlatinum }}</p>
          <p>获得 CP：{{ settlementResult.earnedCopper }}</p>

          <div>
            <h3>完成项目</h3>
            <ul>
              <li v-for="item in settlementResult.completedItems" :key="item">
                {{ item }}
              </li>
            </ul>
          </div>

          <button @click="closeModal">关闭</button>
        </div>
      </template>
    </div>
  </div>
</template>

<style scoped>
.overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.45);
  display: flex;
  justify-content: center;
  align-items: center;
}

.modal {
  width: 460px;
  max-width: 90%;
  background: #1b1a17;
  color: #f0e6d2;
  border: 1px solid #6f5a3a;
  border-radius: 12px;
  padding: 24px;
  box-shadow: 0 12px 28px rgba(0, 0, 0, 0.45);
}

.modal-header {
  margin-bottom: 20px;
}

.modal-header h2,
.question-area h3,
.result-area h2,
.result-area h3 {
  color: #f0e6d2;
}

.current-value,
.modal-header p {
  color: #cdbb95;
}


.question-area input {
  width: 100%;
  padding: 10px 12px;
  margin-top: 12px;
  border: 1px solid #6f5a3a;
  border-radius: 8px;
  background-color: #2a2722;
  color: #f0e6d2;
  box-sizing: border-box;
}


.boolean-buttons button,
.actions button,
.result-area button {
  padding: 8px 16px;
  border: 1px solid #6f5a3a;
  border-radius: 6px;
  background: linear-gradient(to bottom, #5f472c, #3e2f1f);
  color: #f5e6c8;
  cursor: pointer;
}

.boolean-buttons button:hover,
.actions button:hover,
.result-area button:hover {
  background: linear-gradient(to bottom, #735637, #4c3925);
}

.actions button:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.current-value {
  margin-top: 12px;
  color: #666;
}

.actions {
  margin-top: 24px;
  display: flex;
  justify-content: flex-end;
  gap: 12px;
}

.result-area ul {
  padding-left: 20px;
}
</style>