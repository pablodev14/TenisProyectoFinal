# Parte 1 — Products: tienda de tenis

Proyecto basado en MyRealAPI del profesor. Conserva ASP.NET Core, .NET 10, CsvHelper, OpenAPI, Swagger UI y la organización por capas.
El ejemplo de clientes se adaptó a Products: esta entrega no incluye rutas de clientes ni implementa las facturas de los otros integrantes. El ZIP original no fue modificado.

## Abrir en Visual Studio

1. Extrae el ZIP completo en una carpeta donde tengas permiso de escritura.
2. Abre MyRealAPI.slnx con una versión de Visual Studio compatible con .NET 10 y el formato .slnx.
3. Debes tener instalado el SDK de .NET 10 y la carga de trabajo de desarrollo web ASP.NET.
4. Espera a que Visual Studio restaure los paquetes NuGet. La primera vez requiere conexión a Internet.
5. Selecciona MyRealAPI como proyecto de inicio y el perfil http.
6. Ejecuta con F5 o Ctrl+F5. Se abrirá http://localhost:5062/swagger.

Si Visual Studio no reconoce la solución, prueba abrir MyRealAPI/MyRealAPI.csproj; si tampoco reconoce net10.0, actualiza Visual Studio y el SDK.
También puedes abrir una terminal en la carpeta MyRealAPI que contiene el .csproj y ejecutar: dotnet run --launch-profile http.
Swagger está habilitado en Development, como en el proyecto original.

## Qué probar

- GET /api/products: devuelve los cinco productos del CSV (200).
- GET /api/products/1: devuelve los Nike Air Force 1 '07 (200).
- GET /api/products/999: devuelve un mensaje de producto inexistente (404).
- GET /api/products/5: devuelve un producto sin stock; también forma parte del catálogo (200).
- Detén la API, cambia un dato de Data/products.csv y vuelve a iniciarla: la consulta mostrará el nuevo dato.

En Swagger abre una operación, pulsa Try it out y luego Execute.
MyRealAPI.http contiene las tres primeras consultas para ejecutarlas desde Visual Studio.

## Cómo seguir el código

1. Program.cs registra las dependencias, carga el catálogo y habilita Swagger.
2. ProductController recibe las consultas HTTP y decide la respuesta HTTP.
3. IProductService / ProductService consultan el repositorio y convierten los modelos a DTOs.
4. IProductRepository / ProductRepository cargan y guardan los productos con CsvHelper.
5. Product representa los datos internos. ProductResponseDto es la respuesta pública.

Cada clase e interfaz está en su propio archivo. Los métodos Copy evitan que otro módulo cambie por accidente los productos cargados sin guardarlos.
El repositorio es Singleton: se comparte una instancia durante la ejecución. El servicio es Scoped: se crea por petición.

## Catálogo de tenis

Las marcas y los modelos son reales. Los precios en COP, los códigos internos y las existencias son datos ficticios para probar el proyecto; no representan ofertas ni disponibilidad real. Cada fila representa una combinación de modelo, color y talla; Stock indica cuántos pares hay de esa combinación. La talla y el color se indican en Description para conservar el modelo sencillo del proyecto. Para agregar otra talla, usa una fila con Id y Code distintos.

## El archivo CSV

Data/products.csv contiene Id, Code, Name, Description, Price y Stock.
Id es el identificador numérico de las rutas y futuras relaciones; Code es el código comercial, por ejemplo TEN-001.
Los importes son decimal. CsvHelper usa CultureInfo.InvariantCulture: separador coma y punto decimal, sin separadores de miles.
El archivo debe conservar sus encabezados. Los Id y Code no se repiten; precio y stock no pueden ser negativos.
Si falta el archivo o los datos son inválidos, la API informa el error al iniciar para que se corrija el catálogo.
El CSV se lee al iniciar. Para editarlo manualmente, detén primero la API.
La ruta se obtiene desde ContentRootPath, no desde una ruta personal del computador del profesor.

## Conexión con los compañeros

El integrante de Receipt construye el modelo, DTOs, servicio, repositorio y controller de facturas.
El integrante de DetailReceipt construye su modelo, DTOs, servicio y repositorio. El endpoint para agregar detalles pertenece al controller de facturas.
Ambos conservan las carpetas y el namespace MyRealAPI. Registran sus servicios y repositorios en Program.cs.
Sus CSV se ubicarán en Data/receipts.csv y Data/details.csv; deben incluirlos en la entrega final.

Contrato disponible de Products:
- IProductRepository.GetAll(): devuelve una copia del catálogo.
- IProductRepository.Get(id): devuelve una copia del producto o null.
- IProductRepository.UpdateStock(id, stock): guarda el nuevo stock absoluto en CSV y luego actualiza la memoria. Rechaza valores negativos o un producto inexistente.

Ejemplo: si Get(1) devuelve Stock = 12 y se venden 2 unidades, el servicio de ventas debe validar y guardar UpdateStock(1, 10). El segundo argumento no es la cantidad vendida.
Este método no es un endpoint y no permite cambiar precio ni descripción.

Acuerdos recomendados antes de continuar:
- Crear detalle: recibir únicamente ProductId (int) y Quantity (int).
- Crear factura: recibir documento y nombre del cliente; número, fecha, estado y total los decide el servidor.
- Guardar ReceiptId en cada detalle para reconstruir la relación al iniciar.
- Copiar Description y Price al detalle; no consultar el precio actual para recalcular una venta anterior.
- Usar decimal para UnitPrice, Subtotal y Total.
- Responder 404 para recursos inexistentes, 400 para cantidades inválidas y 409 para stock insuficiente o factura cerrada.

IMPORTANTE: UpdateStock guarda solo el catálogo. El servicio que agrega detalles debe coordinar validación, descuento, guardado del detalle y actualización de factura. El bloqueo del repositorio protege cada acceso, pero no convierte una venta entre varios CSV en una transacción. Deben evitar ventas concurrentes que lean el mismo stock y contemplar fallos de guardado antes de completar la integración.
Las reglas de ventas, su persistencia y la inmutabilidad de facturas cerradas corresponden a las siguientes partes.

## Entrega final del equipo

Esta es únicamente la parte 1. Aún faltan Receipt, DetailReceipt, pruebas de integración de ventas y el diagrama UML final en Draw.io que refleje el código de los tres.
No subir bin, obj ni .vs al repositorio. El .gitignore ya los excluye.

## Referencias de los modelos

- Nike Air Force 1 '07: https://www.nike.com/w/air-force-1-shoes-5sj3yzy7ok
- adidas Samba OG: https://www.adidas.com/us/samba-samba_og-shoes
- New Balance 550: https://www.newbalance.com/es/550/
- Vans Old Skool: https://www.vans.com/en-us/c/shoes/icons/old-skool-5205
- Converse Chuck 70: https://www.converse.com/shop/chuck-70-shoes
