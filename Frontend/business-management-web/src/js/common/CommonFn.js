import Resource from "./Resource"
import Enumeration from "./Enumeration"
import moment from "moment"

var CommonFn = CommonFn || {};

/**
 * hàm format số nguyên
 */
CommonFn.formatNumber = number => {
    if (number && !isNaN(number)) {
        return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    } else {
        return number;
    }
}

/**
 * Format ngày tháng từ dữ liệu thô
 * @param {*} dateSrc 
 * @returns 
 */
CommonFn.formatDate = dateSrc => {
    if (dateSrc)
        dateSrc = moment(dateSrc).format("DD/MM/YYYY");

    return dateSrc;
}

/**
 * Format tên 
 * @param {string} name 
 * @returns 
 */
CommonFn.formatName = name => {
    let fullName = name.split(' '),
        res = '';

    fullName.map(item => {
        item = item.charAt(0).toUpperCase() + item.slice(1);
        res += item + ' ';
    });

    return res.trim();
}

/**
 * Hàm lấy dữ liệu từ dang Enum
 */
CommonFn.getEnumValue = (data, enumName) => {
    let enumeration = Enumeration[enumName],
        resource = Resource[enumName];

    for (let propName in enumeration) {
        if (enumeration[propName] == data) {
            data = resource[propName];
        }
    }

    return data;
}

/**
* Hàm chuyển đổi dữ liệu để hiển thị lên bảng
*/
CommonFn.convertOriginData = (data, dataType, enumName) => {
    let temp = '';

    if (data != null) {
        temp = data;

        switch (dataType) {
            case Resource.DataTypeColumn.Number:
                temp = CommonFn.formatNumber(temp);
                break;
            case Resource.DataTypeColumn.Name:
                temp = CommonFn.formatName(temp);
                break;
            case Resource.DataTypeColumn.Date:
                temp = CommonFn.formatDate(temp);
                break;
            case Resource.DataTypeColumn.Enum:
                temp = CommonFn.getEnumValue(temp, enumName);
                break;
        }
    }

    return temp;
}

/**
 * Hàm lấy dữ liệu gốc từ dữ liệu hiển thị
 * @param {string} displayData Dữ liệu hiển thị
 * @param {string} dataType Kiểu dữ liệu
 * @param {*} enumName 
 */
CommonFn.getOriginData = (displayData, dataType, enumName) => {
    let temp = null;

    if (displayData != null) {
        temp = JSON.parse(JSON.stringify(displayData));
        switch (dataType) {
            case Resource.DataTypeColumn.Number:
                temp = CommonFn.getOriginNumber(temp);
                break;
            case Resource.DataTypeColumn.Enum:
                console.log(enumName);
                break;
        }
    }

    return temp;
}

/**
 * Hàm lấy số từ số hiển thị
 * @param {string} number Số dạng hiển thị
 */
CommonFn.getOriginNumber = (number) => {
    return number.toString().length > 0 ? parseFloat(number.toString().replaceAll(',', '')) : null;
}

/**
 * Hàm tạo lỗi bắt buộc nhập
 * @param {string} fieldName Tên trường bị lỗi
 * @returns Thông điệp lỗi bắt buộc nhập
 */
CommonFn.getRequiredErrorMsg = (fieldName) => {
    return fieldName + ' không được để trống';
}

/**
 * Hàm tạo thông điệp lỗi bị trùng
 * @param {string} value Tên trường cùng giá trị bị trùng
 * @returns Thông điệp lỗi tồn tại trong hệ thống
 */
CommonFn.getUniqueErrorMsg = (value) => {
    return value + ' đã tồn tại trong hệ thống, vui lòng kiểm tra lại';
}

CommonFn.getMaxLengthErrorMsg = (fieldName, maxLength) => {
    return fieldName + ' không được vượt quá ' + maxLength + ' ký tự';
}

/**
 * Hàm tạo thông điệp lỗi kiểu dữ liệu
 * @param {string} fieldName Tên trường dữ liệu bị lỗi
 * @returns Thông điệp lỗi
 */
CommonFn.getDataTypeErrorMsg = (fieldName) => {
    return fieldName + ' không hợp lệ';
}

CommonFn.getConflictDataErrorMsg = (value) => {
    return "Không thể xoá " + value + " vì đã được liên kết với dữ liệu khác, vui lòng kiểm tra lại"
}

/**
 * Hàm tính thu nhập tính thuế
 * @param {number} salary Tiền lương
 * @param {number} insuranceMoney Tiền bảo hiểm
 * @returns Thu nhập tính thuế
 */
CommonFn.calculateTaxableIncome = (salary, insuranceMoney) => {
    if (!salary) salary = 0;
    if (!insuranceMoney) insuranceMoney = 0;

    let taxableIncome = salary - insuranceMoney - 11000000;

    return taxableIncome > 0 ? Math.round(taxableIncome * 100) / 100 : 0;
}

/**
 * Tính thuế
 * @param {double} taxableIncome Thu nhập tính thuế
 * @returns Tiền thuế
 */
CommonFn.calculateTaxMoney = (taxableIncome) => {
    let taxMoney = 0;

    if (taxableIncome == 0) taxMoney = 0;
    if (taxableIncome <= 5000000) taxMoney = 0.05 * taxableIncome;
    if (taxableIncome > 5000000 && taxableIncome <= 10000000) taxMoney = 0.1 * taxableIncome - 250000;
    if (taxableIncome > 10000000 && taxableIncome <= 18000000) taxMoney = 0.15 * taxableIncome - 750000;
    if (taxableIncome > 18000000 && taxableIncome <= 32000000) taxMoney = 0.2 * taxableIncome - 1650000;
    if (taxableIncome > 32000000 && taxableIncome <= 52000000) taxMoney = 0.25 * taxableIncome - 3250000;
    if (taxableIncome > 52000000 && taxableIncome <= 80000000) taxMoney = 0.3 * taxableIncome - 5000000;
    if (taxableIncome > 80000000) taxMoney = 0.35 * taxableIncome - 9000000;

    return Math.round(taxMoney * 100) / 100;
}

export default CommonFn;