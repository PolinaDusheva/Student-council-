// ============================================================
// ФАЙЛ: Models/EventAttendee.cs
// ЦЕЛЬ: Модел за участник/записан човек към конкретно събитие.
//       Представлява релация "един към много" — едно събитие (CouncilEvent)
//       може да има много участници (EventAttendee).
//
// РЕЛАЦИЯ:
//   CouncilEvent (1) ──── (N) EventAttendee
//   EventId е Foreign Key — свързва участника с неговото събитие.
// ============================================================

namespace StudentCouncil.Models;

public class EventAttendee
{
    // Първичен ключ — генерира се автоматично от базата (AUTOINCREMENT).
    public int Id { get; set; }

    // Foreign Key — свързва участника с конкретно събитие.
    // EF Core разпознава по конвенция: [ModelName]Id → Foreign Key.
    public int EventId { get; set; }

    // Името на участника — въвежда се ръчно от формата.
    public string Name { get; set; } = "";
}
