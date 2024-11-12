import { defineStore } from 'pinia'
import { ref } from 'vue'
import { FinanceDetails, financeIncome, financeList } from '@/api/services/financeServices'

export const useFinanceStore = defineStore('finance', () => {
  const id = ref(0)
  const strTitle = ref('')
  const expensesList = ref<FinanceDetails[]>([]) // Reactive state using ref
  const incomesList = ref<financeIncome[]>([])
  const editMode = ref<boolean>(false)

  const setId = (newId: number) => {
    id.value = newId
  }

  const setTitle = (newTitle: string) => {
    strTitle.value = newTitle
  }

  const setList = (newList: FinanceDetails[]) => {
    expensesList.value = newList
  }

  const setIncomeList = (newList: financeIncome[]) =>{
    incomesList.value = newList
  }

  const setEditMode = (mode: boolean) => {
    editMode.value = mode
  }

  return { id, strTitle, list: expensesList, incomesList, editMode, setId, setTitle, setList,setIncomeList, setEditMode }
})
