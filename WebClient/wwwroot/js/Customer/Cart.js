document.querySelectorAll('.increase-quantity').forEach(button => {
    button.addEventListener('click', async (event) => {
        const cartItem = event.currentTarget.closest('.cart-item');
        const cartId = cartItem.dataset.id;
        const quantityInput = cartItem.querySelector('.quantity');
        const itemTotal = cartItem.querySelector('.item-total');

        const newQuantity = parseInt(quantityInput.value) + 1;
        const response = await fetch('/Customer/UpdateItem', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ Id: cartId, Quantity: 1 })
        });

        const result = await response.json();
        if (result.success) {
            quantityInput.value = newQuantity;
            const price = parseFloat(itemTotal.dataset.price);
            itemTotal.textContent = (price * newQuantity).toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
            updateTotalPrice();
        }
    });
});

document.querySelectorAll('.decrease-quantity').forEach(button => {
    button.addEventListener('click', async (event) => {
        const cartItem = event.currentTarget.closest('.cart-item');
        const cartId = cartItem.dataset.id;
        const quantityInput = cartItem.querySelector('.quantity');
        const itemTotal = cartItem.querySelector('.item-total');

        const current = parseInt(quantityInput.value);
        if (current <= 1) return;
        const newQuantity = current - 1;
        const response = await fetch('/Customer/UpdateItem', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ Id: cartId, Quantity: -1 })
        });

        const result = await response.json();
        if (result.success) {
            quantityInput.value = newQuantity;
            const price = parseFloat(itemTotal.dataset.price);
            itemTotal.textContent = (price * newQuantity).toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
            updateTotalPrice();
        }
    });
});

document.querySelectorAll('.remove-item').forEach(button => {
    button.addEventListener('click', async (event) => {
        const cartItem = event.currentTarget.closest('.cart-item');
        const cartId = cartItem.dataset.id;

        const response = await fetch(`/Customer/DeleteItem/${cartId}`, {
            method: 'POST'
        });

        const result = await response.json();
        if (result.success) {
            cartItem.remove();
            updateTotalPrice();
        }
    });

});
function updateTotalPrice() {
    let total = 0;
    document.querySelectorAll('.cart-item').forEach(item => {
        const quantity = parseInt(item.querySelector('.quantity').value);
        const price = parseFloat(item.querySelector('.item-total').dataset.price);
        total += quantity * price;
    });

    document.querySelector('.total-price').textContent =
        total.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
}
