# Guia de instalacion

## SSL

- Para usar SSL en el frontend usamos Github Pages para poner el frontend estatico en linea. El mismo es accesible en: <https://kattyab.github.io/CI0137_seg_fault/>

- Para el backend, usamos duckdns para obtener un dominio, lo dirigimos a un servidor el cual usa el reverse proxy Caddy, que automaticamente maneja los certificados SSL con Let's encrypt. El mismo es accesible en: <https://ci0137-seg-fault.duckdns.org/swagger/index.html>

## Servidor

- Se utiliza un servidor con docker instalado, la instalacion de docker esta fuera del alcance de este proyecto.
- El servidor debe tener una IP publica, la cual se asocia con un dominio a traves de duckdns.
En este caso usamos ci0137-seg-fault.duckdns.org, el cual apunta a la direccion IP del servidor.

## Despliegue de aplicacion

- Se pone el dominio del backend en la variable de entorno VITE_API_BASE_URL en el repositorio de github para que el frontend dirija las peticiones de api al servidor correctamente

- El backend permite el origen del frontend por CORS usando variables de entorno. En `compose.yaml` se configura:

```yaml
Cors__AllowedOrigins__0: https://kattyab.github.io
```

Si se necesita permitir otro frontend, se cambia esa URL por el origen exacto del sitio, sin slash final. Para multiples origenes se agregan mas indices:

```yaml
Cors__AllowedOrigins__0: https://kattyab.github.io
Cors__AllowedOrigins__1: http://localhost:5173
```

- Se corre el contenedor con las 3 aplicaciones un contenedor de docker con 3 servicios:
  - app: aplicacion del backend
  - caddy: reverse proxy para el ssl del backend
  - mssql: base de datos para el api

- Para correr el contenedor se utiliza el siguiente comando: `docker compose up -d --build`, este construye una imagen para el api a partir del codigo fuente y corre todos los servicios

## Instalacion de base de datos

- Al tener el servidor de mssql corriendo, se deben correr los scripts para instalar la base de datos de la api

- Se puede confirmar que el servidor corre correctamente con el siguiente comando: `docker compose exec -T mssql /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'Password1' -C -Q "SELECT 1"`

- Se corren los scripts en el directorio database directamente en el contenedor

```bash
docker compose exec -T mssql /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'Password1' -C -b < database/001
docker compose exec -T mssql /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'Password1' -C -b < database/002_inserts_colores_por_carpeta.sql
docker compose exec -T mssql /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'Password1' -C -b < database/003
```

Notas:

- `-T` deshabilita el TTY interactivo para que la redireccion de entrada de consola funcione correctamente.
- `-C` confia en el certificado del servidor, requerido por sqlcmd 18.
- `-b` hace que sqlcmd retoorne un codigo de error si un script falla.
