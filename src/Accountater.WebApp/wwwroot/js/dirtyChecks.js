(() => {
    let isSubmitting = false;
    let initialState = new Map();
    const form = document.querySelector('form');

    // Gather all input/select/textarea elements (can expand later)
    const fields = form.querySelectorAll('input, select, textarea');

    // Store initial values
    fields.forEach(field => {
        const key = getFieldKey(field);
        if (key) {
            initialState.set(key, getFieldValue(field));
        }
    });

    function getFieldKey(field) {
        return field.name || field.id || null;
    }

    function getFieldValue(field) {
        if (field.type === 'checkbox' || field.type === 'radio') {
            return field.checked;
        }
        return field.value;
    }

    function isFormDirty() {
        for (let field of fields) {
            const key = getFieldKey(field);
            if (!key) continue;

            const initial = initialState.get(key);
            const current = getFieldValue(field);
            if (initial !== current) return true;
        }
        return false;
    }

    // Prevent warning on intentional submit
    form.addEventListener('submit', () => {
        isSubmitting = true;
    });

    // Warn on accidental unload
    window.addEventListener('beforeunload', (e) => {
        if (isFormDirty() && !isSubmitting) {
            const message = 'You have unsaved changes! If you leave, your changes will be lost.';
            e.preventDefault();
            e.returnValue = message;
            return message;
        }
    });
})();
