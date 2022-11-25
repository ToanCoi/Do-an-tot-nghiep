var Enumeration = Enumeration || {};

//Giới tính
Enumeration.Gender = {
    Female: 1,
    Male: 0,
    Other: 2
}

//Trạng thái làm việc
Enumeration.WorkStatus = {
    Active: 0,
    Intern: 1,
    Retired: 2
}

//Các chế độ của form
Enumeration.FormMode = {
    Add: 1, 
    Edit: 2,
    Delete: 3,
    Clone: 4,
}

//Loại lỗi input
Enumeration.ErrorType = {
    Require: 1,
    Unique: 2,
    MaxLength: 3,
    DataType: 4
}


Enumeration.MisaCode = {
    Conflict: 999,
}
//Trạng thái của đối tượng
Enumeration.ObjectMode = {
    Insert: 1,
    Update: 2,
    Delete: 3,
}

// Trạng thái của bàn
Enumeration.TableStatus = {
    New: 0,
    InProgress: 1,
}

export default Enumeration;