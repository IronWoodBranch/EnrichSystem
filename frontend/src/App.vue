<script setup>
import { onMounted, ref } from 'vue'

const dashboard = ref(null)
const quests = ref([])
const shop = ref([])
const chronicle = ref([])
const dailyNote = ref('')
const phase = ref('P2')

const loadAll = async () => {
  dashboard.value = await (await fetch('/api/dashboard')).json()
  quests.value = await (await fetch('/api/quests')).json()
  shop.value = await (await fetch('/api/shop')).json()
  chronicle.value = await (await fetch('/api/chronicle')).json()
  phase.value = dashboard.value.phase
}

const completeQuest = async (id) => {
  await fetch(`/api/quests/${id}/complete`, { method: 'POST' })
  await loadAll()
}

const buy = async (id) => {
  const res = await fetch(`/api/shop/${id}/buy`, { method: 'POST' })
  if (!res.ok) alert('购买失败：可能余额不足或未解锁')
  await loadAll()
}

const addDiary = async () => {
  if (!dailyNote.value.trim()) return
  await fetch('/api/chronicle', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ text: dailyNote.value })
  })
  dailyNote.value = ''
  await loadAll()
}

const updatePhase = async () => {
  await fetch('/api/phase', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ phase: phase.value })
  })
  await loadAll()
}

onMounted(loadAll)
</script>

<template>
  <main v-if="dashboard" class="shell">
    <h1>Project Reboot / 指挥终端</h1>
    <section class="panel">
      <p>铜币：{{ dashboard.copper }} ｜ 琥珀币：{{ dashboard.amber }}</p>
      <p>吉他基金：{{ dashboard.guitarFund }}</p>
      <p>回国进度：{{ dashboard.returnHomeProgressPercent.toFixed(1) }}% / 450</p>
      <label>阶段倍率
        <select v-model="phase" @change="updatePhase">
          <option value="P2">P2</option>
          <option value="P3">P3</option>
          <option value="P4">P4</option>
        </select>
      </label>
    </section>

    <section class="panel">
      <h2>Quest Board</h2>
      <article v-for="q in quests" :key="q.id" class="card">
        <strong>{{ q.title }}</strong>
        <p>{{ q.flavorText }}</p>
        <small>{{ q.currency }} +{{ q.reward }} ({{ q.kind }})</small>
        <button :disabled="q.kind === 'Bounty' && q.completed" @click="completeQuest(q.id)">
          {{ q.completed ? '已完成' : '结算' }}
        </button>
      </article>
    </section>

    <section class="panel">
      <h2>Shop</h2>
      <article v-for="i in shop" :key="i.id" class="card row">
        <span>{{ i.name }} - {{ i.price }} {{ i.currency }}</span>
        <button @click="buy(i.id)">购买</button>
      </article>
    </section>

    <section class="panel">
      <h2>The Chronicle</h2>
      <div class="row">
        <input v-model="dailyNote" placeholder="今日文字结项..." />
        <button @click="addDiary">记录</button>
      </div>
      <ul>
        <li v-for="c in chronicle" :key="c.id">{{ c.at.slice(0, 10) }} - {{ c.text }} ({{ c.delta }})</li>
      </ul>
    </section>
  </main>
</template>
