import { defineStore } from 'pinia'
import { reactive } from 'vue'

export const useRegistroStore = defineStore('registro', () => {
  const form = reactive({
    nombre: '',
    email: '',
    telefono: '',
    password: '',
    passwordConfirm: '',
    terminos: false,
  })

  const clearForm = () => {
    form.nombre = ''
    form.email = ''
    form.telefono = ''
    form.password = ''
    form.passwordConfirm = ''
    form.terminos = false
  }

  return { form, clearForm }
})
