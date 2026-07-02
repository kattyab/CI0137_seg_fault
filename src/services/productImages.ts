// Local product images resolved from the assets tree.
//
// INVENTARIO.imagen_url (returned by the API as imagenUrl) stores the colorway
// FOLDER in Vite-alias form, e.g. '@/assets/images/low/1lowG/white-linen';
// the shot filename (right.png, front.png, ...) is appended here. When the API
// gives no imagenUrl, the folder is derived from the product name plus the DB
// color name — COLORES.nombre matches the colorway folder name exactly.
// Collection models have no colorway subfolders: shots live in the model folder.

const files = import.meta.glob('../assets/images/{low,mid,high,collection}/**/*.png', {
  eager: true,
  query: '?url',
  import: 'default',
}) as Record<string, string>

export const SHOTS = [
  'right',
  'left',
  'front',
  'back',
  'up',
  'down',
  'front-detail',
  'back-detail',
] as const
export type Shot = (typeof SHOTS)[number]
const DEFAULT_SHOT: Shot = 'right'

const PREFIX = '../assets/images/'

// Model folder per DB product name (SNEAKERS.nombre). Colorway subfolders are
// discovered from the glob, so only new models need an entry here.
const MODEL_FOLDERS: Record<string, string> = {
  'Air Jordan 1 Low': 'low/1low',
  'Air Jordan 1 Low G': 'low/1lowG',
  'Air Jordan 1 Low SE': 'low/1lowSE',
  'Air Jordan 1 Skyline Low': 'low/skylineLow',
  'Air Jordan 1 Mid': 'mid/1mid',
  'Air Jordan 1 Mid SE': 'mid/1midSE',
  'Air Jordan 1 Mid SE Edge': 'mid/1midSEEdge',
  'Air Jordan 1 Retro High OG': 'high/1retroHighOG',
  'Air Jordan 1 Retro High OG Flight Club': 'high/1retroHighOGFlightClub',
  'Air Jordan 9 Retro High': 'high/9retro',
  'Air Jordan 4 Retro SE We Are Eternal': 'collection/airJordan4RetroSEWeAreEternal',
  'Air Jordan 5 Black Carolina': 'collection/airJordan5BlackCarolina',
  'Air Jordan 7 Retro Miro': 'collection/airJordan7RetroMiro',
  "Book 2x McDonald's": 'collection/book2xMcDonalds',
}

// 'cat/model' -> colorway subfolder names, derived from the glob keys
const colorwaysByModel = new Map<string, string[]>()
for (const key of Object.keys(files)) {
  const parts = key.slice(PREFIX.length).split('/')
  if (parts.length !== 4) continue // model folders without colorways have 3 parts
  const model = `${parts[0]}/${parts[1]}`
  const colorways = colorwaysByModel.get(model) ?? []
  if (!colorways.includes(parts[2]!)) {
    colorways.push(parts[2]!)
    colorwaysByModel.set(model, colorways)
  }
}

const FALLBACK = files[`${PREFIX}collection/airJordan4RetroSEWeAreEternal/${DEFAULT_SHOT}.png`] ?? ''

// '@/assets/images/low/1lowG/white-linen' (DB form) -> 'low/1lowG/white-linen'
const stripBase = (url: string) => url.replace(/^(@\/|\.{0,2}\/)?(src\/)?assets\/images\//, '').replace(/\/+$/, '')

function folderFor(nombre: string, color?: string | null, imagenUrl?: string | null): string | null {
  if (imagenUrl) return stripBase(imagenUrl)
  const model = MODEL_FOLDERS[nombre]
  if (!model) return null
  const colorways = colorwaysByModel.get(model)
  if (!colorways || colorways.length === 0) return model
  if (color && colorways.includes(color)) return `${model}/${color}`
  return `${model}/${colorways[0]}`
}

const shotUrl = (folder: string, shot: Shot) => files[`${PREFIX}${folder}/${shot}.png`] ?? FALLBACK

export function productImage(
  nombre: string,
  color?: string | null,
  imagenUrl?: string | null,
  shot: Shot = DEFAULT_SHOT,
): string {
  const folder = folderFor(nombre, color, imagenUrl)
  return folder ? shotUrl(folder, shot) : FALLBACK
}

// All shots of one colorway, for the product page gallery.
export function productShots(
  nombre: string,
  color?: string | null,
  imagenUrl?: string | null,
): string[] {
  const folder = folderFor(nombre, color, imagenUrl)
  if (!folder) return FALLBACK ? [FALLBACK] : []
  const shots = SHOTS.map((s) => files[`${PREFIX}${folder}/${s}.png`]).filter(
    (u): u is string => Boolean(u),
  )
  if (shots.length > 0) return shots
  return FALLBACK ? [FALLBACK] : []
}

// One representative image per colorway, for category card thumbnails.
export function variantImages(nombre: string): { color: string; url: string }[] {
  const model = MODEL_FOLDERS[nombre]
  if (!model) return []
  const colorways = colorwaysByModel.get(model) ?? []
  return colorways.map((color) => ({ color, url: shotUrl(`${model}/${color}`, DEFAULT_SHOT) }))
}
