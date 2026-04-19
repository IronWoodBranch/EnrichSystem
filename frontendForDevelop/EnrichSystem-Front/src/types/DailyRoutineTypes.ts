export type currencyType = 0|1

export type GetDailyRoutineDto = {
  id: number
  key: string
  name: string
  amount: number
}

/**
 * 页面里真正使用的题目结构
 */
export type Question = {
  id: number
  key: string
  label: string
  amount: number
}

/**
 * 结算完成后抛给父组件的结果
 */
export type SettlementResult = {
  platinum: number
  copper: number
  routineDetails: CompleteDailyRoutineDetailsDto[]
}
export type CompleteDailyRoutineDetailsDto = {
  id: number
  name: string
  amount: number
  isCompleted: boolean
  currencyType: number
}