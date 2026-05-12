namespace Hms.API.DTOs
{
    // ── Response DTO (GET endpoints only — never accepted as input) ───────────
    public class RoomDto
    {
        public int RoomNumber { get; set; }
        public string RoomType { get; set; } = null!;
        public int BlockFloor { get; set; }
        public int BlockCode { get; set; }
        public bool Unavailable { get; set; }
    }

    // ── Write DTO  — shared by POST and PUT ───────────────────────────────────
    // RoomNumber is intentionally excluded: for POST it comes from the route
    // (/api/rooms/{roomNumber}), for PUT it also comes from the route (/api/rooms/{id}).
    // The client can never overwrite the primary key through the request body.
    public class RoomWriteDto
    {
        public string RoomType { get; set; } = null!;
        public int BlockFloor { get; set; }
        public int BlockCode { get; set; }
    }

    // CreateRoomDto        → REMOVED  — merged into RoomWriteDto
    // UpdateRoomDto        → REMOVED  — merged into RoomWriteDto
    // UpdateAvailabilityDto → REMOVED — controller uses [FromBody] bool unavailable
}