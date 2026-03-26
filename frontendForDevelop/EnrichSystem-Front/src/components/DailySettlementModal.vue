<script setup lang = "ts">
    const emit = defineEmits<{
      (e: 'close'): void
    }>()

    function closeModal() {
      emit('close')
    }
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