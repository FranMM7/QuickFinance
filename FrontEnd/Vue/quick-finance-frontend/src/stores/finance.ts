import { defineStore } from 'pinia'
import { ref } from 'vue'
import { FinanceDetails, financeList } from '@/api/services/financeServices'

export const useFinanceStore = defineStore('finance', () => {
  const id = ref(0)
  const strTitle = ref('')
  const list = ref<FinanceDetails[]>([]) // Reactive state using ref
  const editMode = ref<boolean>(false)

  const setId = (newId: number) => {
    id.value = newId
  }

  const setTitle = (newTitle: string) => {
    strTitle.value = newTitle
  }

  const setList = (newList: FinanceDetails[]) => {
    list.value = newList
  }

  const setEditMode = (mode: boolean) => {
    editMode.value = mode
  }

  return { id, strTitle, list, editMode, setId, setTitle, setList, setEditMode }
})
