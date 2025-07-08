// Toast notification system
window.showToast = function(message, type = 'success') {
    // Remove existing toasts
    const existingToasts = document.querySelectorAll('.toast');
    existingToasts.forEach(toast => toast.remove());
    
    // Create toast element
    const toast = document.createElement('div');
    toast.className = `toast ${type}`;
    
    // Add icon based on type
    const icon = type === 'success' ? '✓' : '⚠';
    
    toast.innerHTML = `
        <span class="toast-icon">${icon}</span>
        <span class="toast-message">${message}</span>
    `;
    
    // Add to DOM
    document.body.appendChild(toast);
    
    // Show toast
    setTimeout(() => {
        toast.classList.add('show');
    }, 100);
    
    // Auto-hide after 3 seconds
    setTimeout(() => {
        toast.classList.remove('show');
        setTimeout(() => {
            if (toast.parentNode) {
                toast.remove();
            }
        }, 300);
    }, 3000);
};

// Set up interval for cart count updates
window.setInterval = function(dotNetRef, methodName, interval) {
    return setInterval(() => {
        dotNetRef.invokeMethodAsync(methodName);
    }, interval);
};

// Shopping cart utilities
window.cartUtils = {
    // Scroll to top of page
    scrollToTop: function() {
        window.scrollTo({ top: 0, behavior: 'smooth' });
    },
    
    // Animate cart icon when item is added
    animateCartIcon: function() {
        const cartIcon = document.querySelector('.cart-link .icon-cart');
        if (cartIcon) {
            cartIcon.style.transform = 'scale(1.2)';
            cartIcon.style.color = '#e74c3c';
            
            setTimeout(() => {
                cartIcon.style.transform = 'scale(1)';
                cartIcon.style.color = '';
            }, 200);
        }
    },
    
    // Update cart count in navigation
    updateCartDisplay: function(count) {
        const cartCountElement = document.querySelector('.cart-count');
        if (cartCountElement) {
            cartCountElement.textContent = count;
            cartCountElement.setAttribute('data-count', count);
        }
    }
};

// Initialize when DOM is ready
document.addEventListener('DOMContentLoaded', function() {
    console.log('Shopping cart utilities loaded');
});
