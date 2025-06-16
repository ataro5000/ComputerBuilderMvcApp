
document.addEventListener('DOMContentLoaded', function () {

    function updateBuildSummary() {
        const summaryElements = document.querySelectorAll('[id^="summary-"]'); 
        let totalPrice = 0;

        summaryElements.forEach(summaryElement => {
            const category = summaryElement.id.replace('summary-', ''); 
            const dropdown = document.getElementById(`select-${category}`); 

            if (dropdown) {
                const selectedOption = dropdown.options[dropdown.selectedIndex]; 
                const componentName = selectedOption.textContent.split('(')[0].trim(); 
                const componentPrice = parseFloat(selectedOption.dataset.price || 0); 

                summaryElement.textContent = componentName || 'None'; 
                totalPrice += componentPrice; 
            }
        });

        const totalPriceDisplay = document.getElementById('totalPriceDisplay');
        if (totalPriceDisplay) {
            totalPriceDisplay.textContent = totalPrice.toLocaleString('en-US', { style: 'currency', currency: 'USD' });
        }
    }

    const dropdowns = document.querySelectorAll('[id^="select-"]');
    dropdowns.forEach(dropdown => {
        dropdown.addEventListener('change', updateBuildSummary);
    });
    updateBuildSummary();

    
    function collectSelectedComponentsAndSubmit() {
        const selectedComponents = {};
        const dropdowns = document.querySelectorAll('[id^="select-"]');
        dropdowns.forEach(dropdown => {
            const category = dropdown.id.replace('select-', ''); 
            const selectedOption = dropdown.options[dropdown.selectedIndex];
            const selectedId = selectedOption.value;
            if (selectedId) {
                selectedComponents[category] = parseInt(selectedId, 10); 
            }
        });
    
        if (Object.keys(selectedComponents).length === 0) {
            alert("Please select at least one component for your build.");
            return;
        }
    
        const form = document.querySelector('form[asp-action="BuildAndAddToCart"]') || document.querySelector('form'); // Fallback to generic form selector
        if (!form) {
            console.error('Form element not found. Ensure the form exists and has the correct asp-action attribute.');
            return;
        }
    
        const hiddenInput = document.createElement('input');
        hiddenInput.type = 'hidden';
        hiddenInput.name = 'SelectedComponentIds';
        hiddenInput.value = JSON.stringify(selectedComponents);
        form.appendChild(hiddenInput);
        form.submit(); 
    }

    const addToCartButton = document.querySelector('.btn-computer-success');
    if (addToCartButton) {
        addToCartButton.addEventListener('click', function (event) {
            event.preventDefault(); 
            collectSelectedComponentsAndSubmit(); 
        });
    }

    const addedToCartElements = document.querySelectorAll(".added-to-cart");
    addedToCartElements.forEach(element => {
        element.style.opacity = "0"; 
        element.style.transition = "opacity 0.5s ease-in-out"; 
    });

    const miniCartPreview = document.getElementById('miniCartPreview');
    const toggleMiniCartBtn = document.getElementById('toggleMiniCartBtn');
    const closeMiniCartBtn = document.getElementById('closeMiniCartBtn');
    if (toggleMiniCartBtn) {
        toggleMiniCartBtn.addEventListener('click', function () {
            if (miniCartPreview) {
                const isHidden = miniCartPreview.style.display === 'none' || miniCartPreview.style.display === '';
                miniCartPreview.style.display = isHidden ? 'block' : 'none';
                if (isHidden) {
                    updateCartSummaryDisplay(null);
                }
            }
        });
    }

    if (closeMiniCartBtn) {
        closeMiniCartBtn.addEventListener('click', function () {
            if (miniCartPreview) {
                miniCartPreview.style.display = 'none';
            }
        });
    }

    document.querySelectorAll(".add-to-cart-button").forEach(button => {
        button.addEventListener("click", function (event) {
            event.preventDefault(); 

            const productContainer = this.closest(".product-container, .details-item-container");
            const addedToCartElement = productContainer ? productContainer.querySelector(".added-to-cart") : null;
            const form = this.closest("form");

            if (addedToCartElement) {
                addedToCartElement.style.opacity = "1";
                setTimeout(() => {
                    addedToCartElement.style.opacity = "0";
                }, 2000); 
            }

            const formData = new FormData(form);
            fetch(form.action, {
                method: form.method,
                body: formData
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        const itemNameMatch = data.message.match(/^(.*?) \(x\d+\) added to cart\.$/);
                        const lastAddedItemName = itemNameMatch ? itemNameMatch[1] : 'Item';
                        updateCartSummaryDisplay(lastAddedItemName);

                    } else {
                        console.error('Error adding item to cart:', data.message);
                        updateCartSummaryDisplay(null);
                    }
                })
                .catch(error => {
                    console.error('Error during AJAX request:', error);
                    updateCartSummaryDisplay(null);
                });
        });
    });

    const starContainer = document.getElementById('starRatingContainer');
    const ratingInput = document.getElementById('Rating'); 
    const stars = starContainer ? Array.from(starContainer.getElementsByClassName('rating-star-input')) : [];
    const emptyStarSrc = '/images/ratings/rating-0star.png'; 
    const filledStarSrc = '/images/ratings/rating-1star.png'; 
    let initialSubmittedRating = ratingInput ? parseInt(ratingInput.value) : 0;

    if (isNaN(initialSubmittedRating) || initialSubmittedRating < 0 || initialSubmittedRating > 50 || initialSubmittedRating % 10 !== 0) {
        initialSubmittedRating = 0;
        if (ratingInput) ratingInput.value = "0";
    }

    let displayRating = initialSubmittedRating / 10;
    function updateStarsDisplay(ratingToDisplay) {
        stars.forEach(star => {
            const starNumericValue = parseInt(star.dataset.value); 
            if (starNumericValue <= ratingToDisplay) {
                star.src = filledStarSrc;
            } else {
                star.src = emptyStarSrc;
            }
        });
    }

    if (starContainer && ratingInput) {
        updateStarsDisplay(displayRating); 
        starContainer.addEventListener('mouseover', function (e) {
            if (e.target.classList.contains('rating-star-input')) {
                const hoverValue = parseInt(e.target.dataset.value);
                updateStarsDisplay(hoverValue);
            }
        });


        starContainer.addEventListener('mouseout', function () {
            updateStarsDisplay(displayRating);
        });

        starContainer.addEventListener('click', function (e) {
            if (e.target.classList.contains('rating-star-input')) {
                displayRating = parseInt(e.target.dataset.value); 
                ratingInput.value = displayRating * 10; 
                updateStarsDisplay(displayRating); 
            }
        });
    }
    updateCartSummaryDisplay(null);
});


function updateCartSummaryDisplay(lastAddedItemName) {
    fetch('/Cart/GetCartItemCount')
        .then(response => response.json())
        .then(data => {
            const cartBadge = document.getElementById('cartItemCountBadge');
            if (cartBadge) {
                cartBadge.textContent = data.itemCount || 0;
            }

            const miniCartLastItemEl = document.getElementById('miniCartLastItem');
            const miniCartTotalPriceEl = document.getElementById('miniCartTotalPrice');
            if (miniCartTotalPriceEl) {
                miniCartTotalPriceEl.textContent = data.totalCartPrice || '$0.00';
            }

            if (miniCartLastItemEl) {
                if (lastAddedItemName) {
                    miniCartLastItemEl.textContent = lastAddedItemName;
                } else if (data.itemCount === 0) {
                    miniCartLastItemEl.textContent = 'N/A';
                }
            }
        })
        .catch(error => {
            console.error('Error fetching or processing cart summary:', error);
        });
}
