interface User {
  id: string
  nombre: string
  email: string
  telefono: string
  password: string
  terminos: boolean
  createdAt: string
}

const USERS_STORAGE_KEY = 'users_data'

export const userService = {
  /**
   * Obtener todos los usuarios del localStorage
   */
  getAllUsers(): User[] {
    const data = localStorage.getItem(USERS_STORAGE_KEY)
    return data ? JSON.parse(data) : []
  },

  /**
   * Obtener un usuario por email
   */
  getUserByEmail(email: string): User | undefined {
    const users = this.getAllUsers()
    return users.find((user) => user.email === email)
  },

  /**
   * Crear un nuevo usuario
   */
  createUser(userData: Omit<User, 'id' | 'createdAt'>): User | { error: string } {
    // Validar que no exista usuario con ese email
    if (this.getUserByEmail(userData.email)) {
      return { error: 'El correo electrónico ya está registrado.' }
    }

    const users = this.getAllUsers()
    const newUser: User = {
      id: `user_${Date.now()}`,
      nombre: userData.nombre,
      email: userData.email,
      telefono: userData.telefono,
      password: userData.password,
      terminos: userData.terminos,
      createdAt: new Date().toISOString(),
    }

    users.push(newUser)
    localStorage.setItem(USERS_STORAGE_KEY, JSON.stringify(users))

    return newUser
  },

  /**
   * Validar credenciales de usuario (para login)
   */
  validateCredentials(email: string, password: string): User | null {
    const user = this.getUserByEmail(email)
    if (user && user.password === password) {
      return user
    }
    return null
  },

  /**
   * Exportar todos los usuarios como JSON
   */
  exportUsers(): string {
    const users = this.getAllUsers()
    return JSON.stringify(users, null, 2)
  },

  /**
   * Eliminar todos los usuarios (para desarrollo/testing)
   */
  clearAllUsers(): void {
    localStorage.removeItem(USERS_STORAGE_KEY)
  },
}
