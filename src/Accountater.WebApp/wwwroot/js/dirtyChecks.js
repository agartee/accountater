(() => {
    let isSubmitting = false;
    const form = document.querySelector('form');

    function getFields() {
        return form.querySelectorAll('input, select, textarea');
    }

    const initialState = new Map();
    const initialKeys = new Set();

    // Snapshot initial values and keys
    getFields().forEach(field => {
        const key = getFieldKey(field);
        if (key) {
            initialKeys.add(key);
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
        const currentKeys = new Set();
        for (let field of getFields()) {
            const key = getFieldKey(field);
            if (!key) continue;

            currentKeys.add(key);

            const initial = initialState.get(key);
            const current = getFieldValue(field);
            if (initial !== current) return true;
        }

        // If any fields were removed or added
        if (currentKeys.size !== initialKeys.size) return true;

        for (let key of currentKeys) {
            if (!initialKeys.has(key)) return true;
        }

        return false;
    }

    form.addEventListener('submit', () => {
        isSubmitting = true;
    });

    window.addEventListener('beforeunload', (e) => {
        if (isFormDirty() && !isSubmitting) {
            const message = 'You have unsaved changes! If you leave, your changes will be lost.';
            e.preventDefault();
            e.returnValue = message;
            return message;
        }
    });
})();
