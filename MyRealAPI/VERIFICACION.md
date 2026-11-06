# Verificación

Las siguientes comprobaciones corresponden al código de la entrega inicial, sin cambios de lógica en esta versión. El catálogo fue adaptado posteriormente a una tienda de tenis.

- Compilación de MyRealAPI.slnx: correcta, 0 errores y 0 advertencias.
- HTTP GET /api/products: 200 y cinco productos.
- HTTP GET /api/products/1: 200 y el producto con Id 1.
- HTTP GET /api/products/999: 404.
- OpenAPI expone únicamente las dos rutas de Products.
- Swagger: página HTML, JavaScript y CSS respondieron 200.
- Pruebas del repositorio y servicio: consultas, producto agotado, copias independientes, guardado de stock y recuperación en una nueva instancia.
- Rechazo de stock negativo, producto inexistente, identificadores duplicados y CSV faltante.
- Los errores de actualización comprobados no modificaron el CSV.

Las pruebas de escritura usaron una copia del catálogo: el ZIP conserva los datos de ejemplo del catálogo de tenis.
No se han implementado ni probado ventas, facturas ni persistencia coordinada entre varios CSV.

Verificación del catálogo de tenis: las 12 comprobaciones del repositorio y servicio se ejecutaron nuevamente y pasaron, incluida la lectura del código TEN-001 y la persistencia del stock sobre una copia de prueba.
