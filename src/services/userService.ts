interface User {
  id: string
  nombre: string
  email: string
  telefono: string
  passwordHash: string
  passwordSalt: string
  terminos: boolean
  createdAt: string
}

type LegacyUser = Omit<User, 'passwordHash' | 'passwordSalt'> & {
  // Compatibilidad con registros viejos
  password?: string
  passwordHash?: string
  passwordSalt?: string
}

const USERS_STORAGE_KEY = 'users_data'

const PASSWORD_HASH_ITERATIONS = 100_000
const PASSWORD_SALT_BYTES = 16

const toBase64 = (buffer: ArrayBuffer): string => {
  let binary = ''
  const bytes = new Uint8Array(buffer)
  for (const byte of bytes) binary += String.fromCharCode(byte)
  return btoa(binary)
}

const fromBase64ToBytes = (base64: string): Uint8Array => {
  const binary = atob(base64)
  const bytes = new Uint8Array(binary.length)
  for (let i = 0; i < binary.length; i++) {
    bytes[i] = binary.charCodeAt(i)
  }
  return bytes
}

const hashPassword = async (password: string, salt: Uint8Array): Promise<string> => {
  const enc = new TextEncoder()
  // Copia defensiva: asegura respaldo en ArrayBuffer (evita tipos SharedArrayBuffer en TS)
  const saltCopy = new Uint8Array(salt)
  const saltBuffer = saltCopy.buffer as ArrayBuffer
  const keyMaterial = await crypto.subtle.importKey('raw', enc.encode(password), 'PBKDF2', false, [
    'deriveBits',
  ])
  const bits = await crypto.subtle.deriveBits(
    {
      name: 'PBKDF2',
      salt: saltBuffer,
      iterations: PASSWORD_HASH_ITERATIONS,
      hash: 'SHA-256',
    },
    keyMaterial,
    256,
  )
  return toBase64(bits)
}

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
  async createUser(
    userData: Omit<User, 'id' | 'createdAt' | 'passwordHash' | 'passwordSalt'> & { password: string },
  ): Promise<User | { error: string }> {
    // Validar que no exista usuario con ese email
    if (this.getUserByEmail(userData.email)) {
      return { error: 'El correo electrónico ya está registrado.' }
    }

    const users = this.getAllUsers() as unknown as LegacyUser[]

    const saltBytes = crypto.getRandomValues(new Uint8Array(PASSWORD_SALT_BYTES))
    const passwordHash = await hashPassword(userData.password, saltBytes)
    const passwordSalt = toBase64(saltBytes.buffer)

    const newUser: User = {
      id: `user_${Date.now()}`,
      nombre: userData.nombre,
      email: userData.email,
      telefono: userData.telefono,
      passwordHash,
      passwordSalt,
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
  async validateCredentials(email: string, password: string): Promise<User | null> {
    const users = this.getAllUsers() as unknown as LegacyUser[]
    const user = users.find((u) => u.email === email)
    if (!user) return null

    // Caso legacy: contraseña en texto plano
    if (user.password && (!user.passwordHash || !user.passwordSalt)) {
      if (user.password !== password) return null

      // Upgrade: convertir a hash + salt y re-guardar
      const saltBytes = crypto.getRandomValues(new Uint8Array(PASSWORD_SALT_BYTES))
      const passwordHash = await hashPassword(password, saltBytes)
      const passwordSalt = toBase64(saltBytes.buffer)
      user.passwordHash = passwordHash
      user.passwordSalt = passwordSalt
      delete user.password
      localStorage.setItem(USERS_STORAGE_KEY, JSON.stringify(users))

      return user as unknown as User
    }

    if (!user.passwordHash || !user.passwordSalt) return null

    const saltBytes = fromBase64ToBytes(user.passwordSalt)
    const computedHash = await hashPassword(password, saltBytes)
    if (computedHash !== user.passwordHash) return null

    return user as unknown as User
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
