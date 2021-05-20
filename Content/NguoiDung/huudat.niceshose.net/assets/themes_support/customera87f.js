Huudat.CustomerAddress = {
    toggleEditForm: function (id) {
        var editElement = document.getElementById('edit_address_' + id);
        var viewElement = document.getElementById('view_address_' + id);
        editElement.style.display = editElement.style.display == 'none' ? '' : 'none';
        viewElement.style.display = viewElement.style.display == 'none' ? '' : 'none';
        return false;
    },

    toggleNewForm: function () {
        var addElement = document.getElementById('add_address');
        addElement.style.display = addElement.style.display == 'none' ? '' : 'none';
        return false;
    },

    destroy: function (id, confirmMsg) {
        if (confirm(confirmMsg || "Bạn có chắc muốn xóa sổ địa chỉ này?"))
            Huudat.postLink('/account/deleteAddress/' + id);
    }
}