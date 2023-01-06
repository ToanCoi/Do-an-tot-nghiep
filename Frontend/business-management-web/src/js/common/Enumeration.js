var Enumeration = Enumeration || {};

// Trạng thái của bàn
Enumeration.TableStatus = {
    New: 0,
    InProgress: 1,
}

// Trạng thái món ăn
Enumeration.DishStatus = {
    Ready: 0,
    OutOfStock: 1,
}

// Loại món phục vụ
Enumeration.DishType = {
    Appetizer: 0, // Món khai vị
    MainDish: 1, // Món chính
    Dessert: 2, // Món tráng miệng
    Drink: 4,
}

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

//Trạng thái của đối tượng
Enumeration.ObjectMode = {
    Insert: 1,
    Update: 2,
    Delete: 3,
}

export default Enumeration;