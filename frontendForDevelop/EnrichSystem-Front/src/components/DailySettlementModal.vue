<script setup lang = "ts">
    const emit = defineEmits<{
      (e: 'close'): void
    }>()

    function closeModal() {
      emit('close')
    }

    const DailySettlementList = [
      {key : "sleeptime", question:"昨天几点睡",type:"time"},
      { key: 'roomClean', question: '房间是否整洁？', type: 'boolean' },
      { key: 'japaneseStudy', question: '日语是否学习？', type: 'boolean' },
      { key: 'running', question: '是否跑步', type: 'boolean' },
      { key: 'laundry', question: '洗衣服', type: 'boolean' },
      { key: 'japaneseStudy', question: '扔垃圾', type: 'boolean' },
    ]
</script>


<template>
    <div class="overlay">
        <div class="modal">
            <h2>今日结算</h2>
                  <p>这里以后放你的每日问题。</p>

      <button @click="closeModal">关闭</button>
        </div>
    </div>
</template>

<style scoped>
.overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
}

.modal {
  width: 400px;
  background: white;
  border-radius: 12px;
  padding: 24px;
}
</style>