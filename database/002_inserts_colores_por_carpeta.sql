USE [web-development-26];
GO

/* =========================================================================
   INSERTS - Catálogo de Sneakers (Air Jordan)

   Versión "carpetas como fuente de verdad":
   - Cada color en COLORES se llama EXACTAMENTE igual que su carpeta de
     colorway dentro de assets/images. Así no hay que mapear nombres y el
     imagen_url se construye por concatenación:

         '@/assets/images/{categoria}/{modelo}/' + c.nombre

   - La ruta apunta a la CARPETA del colorway (contiene varias imágenes).
     En el front se concatena el archivo: <img :src="`${imagen_url}/(up, down,back, front, ...).png`">

   - Los modelos de Collection no tienen subcarpetas de color, por lo que
     sus filas de INVENTARIO usan id_color = NULL y el imagen_url apunta
     directo a la carpeta del modelo.
   ========================================================================= */

-- =========================================================================
-- 1. CATEGORIAS
-- =========================================================================
INSERT INTO CATEGORIAS (nombre) VALUES
('Low'),
('Mid'),
('High'),
('Collection');

-- =========================================================================
-- 2. TALLAS (según imagen de guía de tallas)
-- =========================================================================
INSERT INTO TALLAS (nombre) VALUES
('W 5 / M 3.5'),
('W 5.5 / M 4'),
('W 6 / M 4.5'),
('W 6.5 / M 5'),
('W 7 / M 5.5'),
('W 7.5 / M 6'),
('W 8 / M 6.5'),
('W 8.5 / M 7'),
('W 9 / M 7.5'),
('W 9.5 / M 8'),
('W 10 / M 8.5'),
('W 10.5 / M 9'),
('W 11 / M 9.5');

-- =========================================================================
-- 3. COLORES
--    Un registro por carpeta de colorway, con el MISMO nombre de la carpeta.
-- =========================================================================
INSERT INTO COLORES (nombre) VALUES
-- images/low/1low
('midnightNavy-white-universityBlue'),
('neutralGrey-summitWhite-infrared23-black'),
('white'),
-- images/low/1lowG
('mediumGray-tintBlue-white'),
('white-linen'),
('white-tintBlue-blueLegend'),
-- images/low/1lowSE
('enigmaStone-paleIvory-hemp-oatmeal'),
('offWhite-softPearl-lightSmokeGrey'),
('teamRed-sawRed'),
-- images/low/skylineLow
('summitWhite-lightSmokeGray-neutralGray-black'),
('summitWhite-universityRed-lightSmokeGray-black'),
-- images/mid/1mid
('black-aura-white-squadronBlue'),
('black-gymRed-black'),
('black-palomino-universityRed-phantom'),
('trueBlue-varsityRed-black-summitWhite'),
-- images/mid/1midSE
('genuineRed-white-courtPurple-black'),
('multicolor-white-wolfGray-black'),
('oliveAura-meadow-universityBlue-pearlWhite'),
('teamRed-villageRed-paleIvory-mountainRed'),
-- images/mid/1midSEEdge
('paleIvory-black-muslin-carreraPink'),
-- images/high/1retroHighOG
('coolGrey-sail-gameRoyal-black'),
('khaki-sail-cargoKhaki-multicolor'),
('paleIvory-coconutMilk-psychicBlue'),
-- images/high/9retro
('black-gumLightBrown-black');

-- =========================================================================
-- 4. SNEAKERS
--    id_categoria: 1=Low, 2=Mid, 3=High, 4=Collection
--    Collection actualizado para coincidir con las carpetas de
--    images/collection. REVISAR precios y descripciones.
-- =========================================================================
INSERT INTO SNEAKERS (id_categoria, nombre, precio, descripcion) VALUES
(1, 'Air Jordan 1 Low',                        65000.00, NULL),
(1, 'Air Jordan 1 Low G',                      75000.00, NULL),
(1, 'Air Jordan 1 Low SE',                     70000.00, NULL),
(1, 'Air Jordan 1 Skyline Low',                90000.00, NULL),
(2, 'Air Jordan 1 Mid',                        70000.00, NULL),
(2, 'Air Jordan 1 Mid SE',                     85000.00, NULL),
(2, 'Air Jordan 1 Mid SE Edge',                80000.00, NULL),
(3, 'Air Jordan 1 Retro High OG',              80000.00, NULL),
(3, 'Air Jordan 9 Retro High',                 85000.00, NULL),
(3, 'Air Jordan 1 Retro High OG Flight Club',  80000.00, NULL),
-- Collection (nombres tomados de las carpetas de images/collection)
(4, 'Air Jordan 4 Retro SE We Are Eternal',   120000.00, 'Edición Limitada'),
(4, 'Air Jordan 5 Black Carolina',            150000.00, 'Edición Limitada'),
(4, 'Air Jordan 7 Retro Miro',                110000.00, 'Edición Limitada'),
(4, 'Book 2x McDonald''s',                    105000.00, 'Edición Limitada');

-- =========================================================================
-- 5. INVENTARIO
--    Cada sneaker x cada uno de sus colorways x las 13 tallas, stock = 10.
--    Como los colores se llaman igual que las carpetas, el imagen_url se
--    arma concatenando la ruta base del modelo + el nombre del color.
-- =========================================================================

-- ---- Air Jordan 1 Low -> images/low/1low ----
INSERT INTO INVENTARIO (id_sneaker, id_color, id_talla, stock, imagen_url)
SELECT s.id, c.id, t.id, 10, '@/assets/images/low/1low/' + c.nombre
FROM SNEAKERS s
CROSS JOIN TALLAS t
CROSS JOIN COLORES c
WHERE s.nombre = 'Air Jordan 1 Low'
  AND c.nombre IN ('midnightNavy-white-universityBlue',
                   'neutralGrey-summitWhite-infrared23-black',
                   'white');

-- ---- Air Jordan 1 Low G -> images/low/1lowG ----
INSERT INTO INVENTARIO (id_sneaker, id_color, id_talla, stock, imagen_url)
SELECT s.id, c.id, t.id, 10, '@/assets/images/low/1lowG/' + c.nombre
FROM SNEAKERS s
CROSS JOIN TALLAS t
CROSS JOIN COLORES c
WHERE s.nombre = 'Air Jordan 1 Low G'
  AND c.nombre IN ('mediumGray-tintBlue-white',
                   'white-linen',
                   'white-tintBlue-blueLegend');

-- ---- Air Jordan 1 Low SE -> images/low/1lowSE ----
INSERT INTO INVENTARIO (id_sneaker, id_color, id_talla, stock, imagen_url)
SELECT s.id, c.id, t.id, 10, '@/assets/images/low/1lowSE/' + c.nombre
FROM SNEAKERS s
CROSS JOIN TALLAS t
CROSS JOIN COLORES c
WHERE s.nombre = 'Air Jordan 1 Low SE'
  AND c.nombre IN ('enigmaStone-paleIvory-hemp-oatmeal',
                   'offWhite-softPearl-lightSmokeGrey',
                   'teamRed-sawRed');

-- ---- Air Jordan 1 Skyline Low -> images/low/skylineLow ----
INSERT INTO INVENTARIO (id_sneaker, id_color, id_talla, stock, imagen_url)
SELECT s.id, c.id, t.id, 10, '@/assets/images/low/skylineLow/' + c.nombre
FROM SNEAKERS s
CROSS JOIN TALLAS t
CROSS JOIN COLORES c
WHERE s.nombre = 'Air Jordan 1 Skyline Low'
  AND c.nombre IN ('summitWhite-lightSmokeGray-neutralGray-black',
                   'summitWhite-universityRed-lightSmokeGray-black');

-- ---- Air Jordan 1 Mid -> images/mid/1mid ----
INSERT INTO INVENTARIO (id_sneaker, id_color, id_talla, stock, imagen_url)
SELECT s.id, c.id, t.id, 10, '@/assets/images/mid/1mid/' + c.nombre
FROM SNEAKERS s
CROSS JOIN TALLAS t
CROSS JOIN COLORES c
WHERE s.nombre = 'Air Jordan 1 Mid'
  AND c.nombre IN ('black-aura-white-squadronBlue',
                   'black-gymRed-black',
                   'black-palomino-universityRed-phantom',
                   'trueBlue-varsityRed-black-summitWhite');

-- ---- Air Jordan 1 Mid SE -> images/mid/1midSE ----
INSERT INTO INVENTARIO (id_sneaker, id_color, id_talla, stock, imagen_url)
SELECT s.id, c.id, t.id, 10, '@/assets/images/mid/1midSE/' + c.nombre
FROM SNEAKERS s
CROSS JOIN TALLAS t
CROSS JOIN COLORES c
WHERE s.nombre = 'Air Jordan 1 Mid SE'
  AND c.nombre IN ('genuineRed-white-courtPurple-black',
                   'multicolor-white-wolfGray-black',
                   'oliveAura-meadow-universityBlue-pearlWhite',
                   'teamRed-villageRed-paleIvory-mountainRed');

-- ---- Air Jordan 1 Mid SE Edge -> images/mid/1midSEEdge ----
INSERT INTO INVENTARIO (id_sneaker, id_color, id_talla, stock, imagen_url)
SELECT s.id, c.id, t.id, 10, '@/assets/images/mid/1midSEEdge/' + c.nombre
FROM SNEAKERS s
CROSS JOIN TALLAS t
CROSS JOIN COLORES c
WHERE s.nombre = 'Air Jordan 1 Mid SE Edge'
  AND c.nombre = 'paleIvory-black-muslin-carreraPink';

-- ---- Air Jordan 1 Retro High OG -> images/high/1retroHighOG ----
INSERT INTO INVENTARIO (id_sneaker, id_color, id_talla, stock, imagen_url)
SELECT s.id, c.id, t.id, 10, '@/assets/images/high/1retroHighOG/' + c.nombre
FROM SNEAKERS s
CROSS JOIN TALLAS t
CROSS JOIN COLORES c
WHERE s.nombre = 'Air Jordan 1 Retro High OG'
  AND c.nombre IN ('coolGrey-sail-gameRoyal-black',
                   'khaki-sail-cargoKhaki-multicolor',
                   'paleIvory-coconutMilk-psychicBlue');

-- ---- Air Jordan 1 Retro High OG Flight Club -> images/high/1retroHighOGFlightClub ----
-- REVISAR: en la captura no se ve si esta carpeta tiene subcarpetas de
-- colorway. Se asume que las imágenes están directo en la carpeta del
-- modelo (id_color = NULL). Si tiene subcarpetas, ajustar igual que arriba.
INSERT INTO INVENTARIO (id_sneaker, id_color, id_talla, stock, imagen_url)
SELECT s.id, NULL, t.id, 10, '@/assets/images/high/1retroHighOGFlightClub'
FROM SNEAKERS s
CROSS JOIN TALLAS t
WHERE s.nombre = 'Air Jordan 1 Retro High OG Flight Club';

-- ---- Air Jordan 9 Retro High -> images/high/9retro ----
INSERT INTO INVENTARIO (id_sneaker, id_color, id_talla, stock, imagen_url)
SELECT s.id, c.id, t.id, 10, '@/assets/images/high/9retro/' + c.nombre
FROM SNEAKERS s
CROSS JOIN TALLAS t
CROSS JOIN COLORES c
WHERE s.nombre = 'Air Jordan 9 Retro High'
  AND c.nombre = 'black-gumLightBrown-black';

-- =========================================================================
-- COLLECTION -> images/collection
-- Los modelos de Collection no tienen subcarpetas de colorway: las imágenes
-- están directo en la carpeta del modelo. Por eso id_color = NULL y el
-- imagen_url apunta a la carpeta del modelo.
-- =========================================================================

INSERT INTO INVENTARIO (id_sneaker, id_color, id_talla, stock, imagen_url)
SELECT s.id, NULL, t.id, 10, '@/assets/images/collection/airJordan4RetroSEWeAreEternal'
FROM SNEAKERS s
CROSS JOIN TALLAS t
WHERE s.nombre = 'Air Jordan 4 Retro SE We Are Eternal';

INSERT INTO INVENTARIO (id_sneaker, id_color, id_talla, stock, imagen_url)
SELECT s.id, NULL, t.id, 10, '@/assets/images/collection/airJordan5BlackCarolina'
FROM SNEAKERS s
CROSS JOIN TALLAS t
WHERE s.nombre = 'Air Jordan 5 Black Carolina';

INSERT INTO INVENTARIO (id_sneaker, id_color, id_talla, stock, imagen_url)
SELECT s.id, NULL, t.id, 10, '@/assets/images/collection/airJordan7RetroMiro'
FROM SNEAKERS s
CROSS JOIN TALLAS t
WHERE s.nombre = 'Air Jordan 7 Retro Miro';

INSERT INTO INVENTARIO (id_sneaker, id_color, id_talla, stock, imagen_url)
SELECT s.id, NULL, t.id, 10, '@/assets/images/collection/book2xMcDonalds'
FROM SNEAKERS s
CROSS JOIN TALLAS t
WHERE s.nombre = 'Book 2x McDonald''s';
