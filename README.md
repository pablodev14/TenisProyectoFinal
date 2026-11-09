<div align="center">

# 👟 STREETWEAR API
### Catálogo urbano · Facturación · Control de stock

Una API REST para una tienda de sneakers, construida con **C# y ASP.NET Core**.

![.NET](https://img.shields.io/badge/.NET-10-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![CSharp](https://img.shields.io/badge/C%23-Backend-239120?style=for-the-badge)
![Swagger](https://img.shields.io/badge/Swagger-API_Docs-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)
![CSV](https://img.shields.io/badge/Persistencia-CSV-00B4D8?style=for-the-badge)

**Proyecto final · Paradigmas de Programación · Equipo de 3 integrantes**

**Products y Receipt disponibles · Registro de ventas en desarrollo**

</div>

---

## ✨ Sobre el proyecto

**Streetwear API** es un proyecto académico para gestionar una tienda de tenis mediante HTTP. La solución final permitirá consultar el catálogo, abrir facturas, agregar productos y cerrar ventas, controlando las existencias y conservando los precios históricos.

El catálogo usa modelos reales de Nike, adidas, New Balance, Vans y Converse. Los precios, códigos internos y existencias son datos ficticios de prueba.

> Desarrollado sobre **MyRealAPI**, el proyecto base proporcionado por el profesor. Streetwear API es el nombre de presentación; la solución y los namespaces conservan MyRealAPI.

## 📦 Avance del proyecto

| Módulo | Qué se puede probar |
| --- | --- |
| ✅ Products | Consultar los cinco modelos del catálogo por lista o por ID. |
| ✅ Receipt | Abrir, listar y recuperar facturas. Consultar sus detalles guardados, calcular el total y cerrar una factura con detalles. |
| ✅ DetailReceipt | Registra la venta, guardar su detalle y descontar el stock en una sola operación coordinada. |

## 🧾 Módulo 2 · Receipt

Una factura nueva recibe un número consecutivo, la fecha del servidor, el estado `Abierta` y un total inicial de cero. El cliente envía su documento y su nombre. Ambos son obligatorios. El DTO de entrada rechaza propiedades adicionales como `total`, `status` o `price`.

El flujo de una solicitud pasa por **ReceiptsController → IReceiptService / ReceiptService → IReceiptRepository / ReceiptRepository**. El servicio decide si se puede cerrar la factura y el repositorio guarda el cambio con CsvHelper. La numeración continúa desde el mayor ID guardado al reiniciar.

Al consultar una factura, Receipt selecciona las líneas con su `ReceiptId`. Cada subtotal usa el precio histórico de la línea multiplicado por su cantidad. El total de una factura abierta es la suma de esos subtotales. Para cerrar debe existir al menos una línea, incluso si su precio es cero. Al cerrar, se guarda el total y el estado pasa a `Cerrada`.

### Archivos de datos

La carpeta `MyRealAPI/MyRealAPI/Data/` contiene los tres CSV. Se distribuyen cinco productos y únicamente los encabezados de facturas y detalles.

| Archivo | Columnas | Uso |
| --- | --- | --- |
| `products.csv` | Id, Code, Name, Description, Price, Stock | Catálogo streetwear. `UpdateStock(id, stock)` recibe la cantidad **final** de pares. |
| `receipts.csv` | Id, Date, CustomerDocument, CustomerName, Status, Total | Receipt guarda cada apertura y cada cierre. |
| `details.csv` | Id, ReceiptId, ProductId, Description, UnitPrice, Quantity | Receipt lee las líneas históricas. El módulo 3 se encargará de escribirlas al vender. |

El repositorio de facturas escribe primero un archivo temporal y después reemplaza el CSV. En una instancia de la API, el bloqueo del repositorio protege la asignación del ID y la escritura frente a solicitudes simultáneas.

## 🌐 Endpoints

| Método | Ruta | Resultado |
| --- | --- | --- |
| GET | `/api/products` | Catálogo completo. |
| GET | `/api/products/{id}` | Producto solicitado o `404`. |
| GET | `/api/receipts` | Facturas registradas. |
| GET | `/api/receipts/{id}` | Factura, detalles y total. Devuelve `404` si el ID no existe. |
| POST | `/api/receipts` | `201` al abrir. Devuelve `400` si los datos son inválidos. |
| POST | `/api/receipts/{id}/close` | `200` al cerrar, `404` si no existe o `409` si está vacía o ya cerrada. |
| POST | `/api/receipts/{id}/details` | Ruta reservada para el módulo 3. Devuelve `501` si está abierta, `404` si no existe y `409` si ya está cerrada. |

## 🚀 Abrir en Visual Studio

1. Extrae el ZIP completo y abre **MyRealAPI/MyRealAPI.slnx** con Visual Studio y el SDK de .NET 10.
2. Espera la restauración de paquetes NuGet y selecciona el perfil **http**.
3. Ejecuta con **Ctrl + F5** y abre [Swagger](http://localhost:5000/swagger).

Desde la raíz del repositorio también puedes usar la terminal.

```bash
dotnet run --project MyRealAPI/MyRealAPI/MyRealAPI.csproj --launch-profile http
```

## 🧪 Probar Receipt en Swagger

1. Ejecuta **POST /api/receipts** con el siguiente cuerpo. Espera `201` y anota el `id` de la respuesta.

   ```json
   {
     "customerDocument": "PRUEBA-001",
     "customerName": "Cliente de prueba"
   }
   ```

2. Usa ese ID en **POST /api/receipts/{id}/close**. Como la factura está vacía, debe responder `409`.
3. Detén la API, ejecútala de nuevo y consulta **GET /api/receipts/{id}**. Debe responder `200` con el mismo cliente, estado `Abierta`, total `0` y `details: []`.
4. Envía `{}` a **POST /api/receipts**. Debe devolver `400`. Prueba también un nombre que contenga solo espacios.
5. Consulta el producto `1` y el `5`. El primero es Nike Air Force 1 '07 con 12 pares y el quinto es Converse Chuck 70 con stock cero.

**Prueba del cierre con detalles.** En una copia de prueba se cargaron dos líneas de la misma factura: dos pares a 12,50 y uno a 5,25. La API devolvió **30,25**, permitió cerrar la factura y conservó estado y total tras reiniciar. También aceptó cerrar una factura con una línea de precio cero y rechazó un segundo cierre. Estas líneas se usaron únicamente para verificar Receipt.

## 👟 Catálogo de ejemplo

| Código | Modelo | Precio (COP) | Pares |
| --- | --- | ---: | ---: |
| TEN-001 | Nike Air Force 1 '07 | 550.000 | 12 |
| TEN-002 | adidas Samba OG | 520.000 | 20 |
| TEN-003 | New Balance 550 | 620.000 | 6 |
| TEN-004 | Vans Old Skool | 320.000 | 8 |
| TEN-005 | Converse Chuck 70 | 380.000 | 0 |

## 🤝 Próxima integración

El integrante de **DetailReceipt** conectará la ruta de agregar productos. El cliente enviará `productId` y `quantity`. El servicio comprobará que la factura siga abierta, validará cantidad y stock y copiará el precio y la descripción del producto a la línea. Al integrar, el equipo deberá coordinar cierre, escritura del detalle y descuento de stock para que las ventas simultáneas o los fallos de guardado no dejen datos parciales.

Quedan pendientes el diagrama UML en Draw.io y la sustentación individual. [Enunciado del proyecto final](https://paradigmas-talleres.pages.dev/#/4/3)

