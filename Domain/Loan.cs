using System;

public class Loan
{
    public required Guid ID { get; set; } = Guid.NewGuid();
    public required Guid ClientID { get; set; }
    public required Guid InventoryID { get; set; }

    public int Days_to_expire { get; set; } = 30;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public DateTime ReturnAt { get; set; }
}
//     public object Days { get; internal set; }

//     public Loan()
//     {
//         ReturnAt = CreatedAt.AddDays(Days_to_expire);
//     }
// }