// Local product images mapped by DB product name and color.
// Temporary bridge until the backend returns imagenUrl from the database:
// a non-null imagenUrl from the API always takes precedence over this manifest.

const files = import.meta.glob('../assets/images/{low,mid,high,collection}/*/*.png', {
  eager: true,
  query: '?url',
  import: 'default',
}) as Record<string, string>

const FALLBACK = files['../assets/images/collection/3retroSailandJadeAura/image.png'] ?? ''

const file = (rel: string) => files[`../assets/images/${rel}`] ?? FALLBACK

type ImageEntry = {
  folder: string
  defaultFile: string
  // exact DB color name -> filename inside the folder
  colors: Record<string, string>
}

// Keys are the exact `nombre` values seeded in database/002
const MANIFEST: Record<string, ImageEntry> = {
  'Air Jordan 1 Low': {
    folder: 'low/1low',
    defaultFile: 'gray-white-black.png',
    colors: {
      'Negro/Blanco/Rojo': 'gray-white-black.png',
      'Blanco/Blanco': 'white.png',
      'Azul Marino/Blanco': 'white-blue-light blue.png',
    },
  },
  'Air Jordan 1 Low G': {
    folder: 'low/1lowG',
    defaultFile: 'gray.png',
    colors: {
      'Gris/Blanco': 'gray.png',
      'Beige/Blanco': 'light brown.png',
      'Blanco/Celeste': 'neon-white.png',
    },
  },
  'Air Jordan 1 Low SE': {
    folder: 'low/1lowSE',
    defaultFile: 'black.png',
    colors: {
      'Negro Charol': 'black.png',
      'Beige/Gris': 'brown.png',
      'Vino/Negro': 'wine.png',
    },
  },
  'Air Jordan 1 Skyline Low': {
    folder: 'low/skylineLow',
    defaultFile: 'red-white-gray.png',
    colors: {
      'Blanco/Rojo': 'red-white-gray.png',
      'Gris/Negro': 'white-black-gray.png',
    },
  },
  'Air Jordan 1 Mid': {
    folder: 'mid/1mid',
    defaultFile: 'white-black-red.png',
    colors: {
      'Rojo/Negro/Blanco': 'white-black-red.png',
      'Gris/Blanco/Negro': 'white-black-light blue.png',
      'Gris Camuflado/Blanco': 'green.png',
    },
  },
  'Air Jordan 1 Mid SE': {
    folder: 'mid/1midSE',
    defaultFile: 'light green.png',
    colors: {
      'Crema/Verde Oliva': 'light green.png',
      'Beige/Rosado': 'brown.png',
      'Rosado/Blanco': 'wine.png',
      'Azul/Blanco': 'blue.png',
    },
  },
  'Air Jordan 1 Mid SE Edge': {
    folder: 'mid/1midSEEdge',
    defaultFile: 'pink-light brown-black.png',
    colors: {
      'Negro/Beige/Naranja-Rosado': 'pink-light brown-black.png',
    },
  },
  'Air Jordan 1 Retro High OG': {
    folder: 'high/1retroHighOG',
    defaultFile: 'gray.png',
    colors: {
      'Gris/Blanco/Azul': 'gray.png',
      'Blanco/Celeste Jaspeado': 'light blue-light brown.png',
    },
  },
  'Air Jordan 9 Retro High': {
    folder: 'high/9retro',
    defaultFile: 'black.png',
    colors: {
      'Negro Jaspeado': 'black.png',
      'Beige/Café': 'brown.png',
    },
  },
  'Air Jordan 3 Retro Sail and Jade Aura': {
    folder: 'collection/3retroSailandJadeAura',
    defaultFile: 'image.png',
    colors: {
      'Sail/Jade con Rosado': 'image.png',
    },
  },
  'Air Jordan 9 Retro Flit Grey and French Blue': {
    folder: 'collection/9retroFlitGreyandFrenchBlue',
    defaultFile: 'image.png',
    colors: {
      'Gris/Azul Francés': 'image.png',
    },
  },
  'Air Jordan MVP 92': {
    folder: 'collection/MVP92',
    defaultFile: 'black-light blue.png',
    colors: {
      'Negro/Turquesa': 'black-light blue.png',
      'Blanco/Morado': 'multi-color.png',
      'Vino/Negro/Turquesa': 'red.png',
    },
  },
  'Air Jordan Spizike': {
    folder: 'collection/spizikeG',
    defaultFile: 'gray.png',
    colors: {
      'Gris Claro/Blanco': 'gray.png',
      'Blanco/Verde/Rojo': 'white-red-green.png',
    },
  },
}

export function productImage(nombre: string, color?: string | null, imagenUrl?: string | null): string {
  if (imagenUrl) return imagenUrl
  const entry = MANIFEST[nombre]
  if (!entry) return FALLBACK
  const filename = (color && entry.colors[color]) || entry.defaultFile
  return file(`${entry.folder}/${filename}`)
}

export function variantImages(nombre: string): { color: string; url: string }[] {
  const entry = MANIFEST[nombre]
  if (!entry) return []
  return Object.entries(entry.colors).map(([color, filename]) => ({
    color,
    url: file(`${entry.folder}/${filename}`),
  }))
}
