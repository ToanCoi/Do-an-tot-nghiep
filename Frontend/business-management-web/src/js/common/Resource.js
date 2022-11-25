var Resource = Resource || {};

// Trạng thái của bàn
Resource.TableStatus = {
    New: "New",
    InProgress: "In progress",
}

//Kiểu dữ liệu của cột trong grid
Resource.DataTypeColumn = {
    Number: "Number",
    Date: "Date",
    Enum: "Enum",
    Email: "Email",
    Name: "Name",
    DateForm: "DateForm",
    OrderNumber: "OrderNumber"
}

//Giới tính
Resource.Gender = {
    Male: "Nam",
    Female: "Nữ",
    Other: "Khác"
}

//Trạng thái làm việc
Resource.WorkStatus = {
    Active: "Đang làm việc",
    Intern: "Đang thử việc",
    Retired: "Đã nghỉ việc"
}

//Các chế độ của form
Resource.FormType = {
    Add: "Add",
    Edit: "Edit",
    Delete: "Delete",
    Refresh: "Refresh"
}

//Tin nhắn trả về người dùng
Resource.Message = {
    AddSuccess: "Thêm dữ liệu thành công",
    EditSuccess: "Cập nhật dữ liệu thành công",
    DeleteSuccess: "Xoá dữ liệu thành công",
    ServerError: "Có lỗi xảy ra, vui lòng liên hệ MISA",
    GetDepartmentError: "Có lỗi khi lấy phòng ban, vui lòng liên hệ MISA",
    GetNewCodeError: "Không thể lấy mã mới, vui lòng liên hệ MISA",
    SelectAtLeastOne: "Vui lòng chọn ít nhất một bản ghi",
    RequiredInputCell: "Vui lòng điền đầy đủ thông tin lương,bảo hiểm",
    DataConflict: "Không thể xoá dữ liệu đã được liên kết, vui lòng kiểm tra lại"
}

//Các chế độ toast message
Resource.Toast = {
    Success: "success",
    Warning: "warning",
    Error: "error"
}

//Các chế độ của popup
Resource.Popup = {
    Warning: "warning",
    Danger: "danger"
}

export default Resource;