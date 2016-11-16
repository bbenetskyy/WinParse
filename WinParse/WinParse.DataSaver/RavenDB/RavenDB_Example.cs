/****************************************************************/
/****************************************************************/
/*************        RavenDB example            ****************/
/************* https://habrahabr.ru/post/113571/ ****************/
/****************************************************************/

//using (var session = store.OpenSession())
//{
//    var order = session.Load<Order>("orders/1");
//    Console.WriteLine("Customer: {0}", order.Customer);
//    foreach (OrderLine orderLine in order.OrderLines)
//    {
//        Console.WriteLine("Product: {0} x {1}", orderLine.ProductId, orderLine.Quantity);
//    }
//}

//using (var session = store.OpenSession())
//{
//    var product = new Product
//    {
//        Cost = 3.99m,
//        Name = "Milk",
//    };
//    session.Store(product);
//    session.SaveChanges();

//    session.Store(new Order
//    {
//        Customer = "customers/ayende",
//        OrderLines =
//        {
//            new OrderLine
//            {
//                ProductId = product.Id,
//                Quantity = 3
//            },
//        }
//    });
//    session.SaveChanges();
//}

//store.DatabaseCommands.PutIndex("OrdersContainingProduct", new IndexDefinition
//{
//  Map = orders => from order in orders
//          from line in order.OrderLines
//          select new { line.ProductId }
//});

//using (var session = store.OpenSession())
//{
//  var orders = session.LueneQuery("OrdersContainingProduct")
//   .Where("ProductId:products/1")
//   .ToArray();

//  foreach (var order in orders)
//  {
//   Console.WriteLine("Id: {0}", order.Id);
//   Console.WriteLine("Customer: {0}", order.Customer);
//   foreach (var orderLine in order.OrderLines)
//   {
//    Console.WriteLine("Product: {0} x {1}", orderLine.ProductId, orderLine.Quantity);
//   }
//  }
//}
/****************************************************************/
/****************************************************************/
/****************************************************************/
/****************************************************************/