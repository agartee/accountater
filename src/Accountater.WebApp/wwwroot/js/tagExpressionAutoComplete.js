(() => {
    const modelHints = {
        financialTransaction: {
            account: {
                name: {
                    _type: 'string',
                    _methods: ['charAt(index)', 'concat(string)', 'includes(searchString)', 'startsWith(searchString)', 'endsWith(searchString)']
                },
                description: {
                    _type: 'string',
                    _methods: ['charAt(index)', 'concat(string)', 'includes(searchString)', 'startsWith(searchString)', 'endsWith(searchString)']
                }
            },
            date: {
                _type: 'date',
                _methods: ['getDay()', 'getMonth()', 'getFullYear()']
            },
            description: {
                _type: 'string',
                _methods: ['includes(searchString)', 'startsWith(searchString)', 'endsWith(searchString)']
            },
            amount: null,
            tags: {
                _type: "array",
                _methods: ["includes(string)"]
            }
        }
    };

    var editor = CodeMirror.fromTextArea(document.getElementById('Expression'), {
        lineNumbers: true,
        mode: 'javascript',
        extraKeys: {
            'Ctrl-Space': 'autocomplete'
        },
        hintOptions: { hint: customHint }
    });

    editor.on('inputRead', function (instance, change) {
        if (change.text[0] === '.' && change.origin === '+input') {
            instance.showHint({ hint: customHint, completeSingle: false });
        }
    });

    function customHint(editor) {
        const cursor = editor.getCursor();
        const token = editor.getTokenAt(cursor);
        const line = editor.getLine(cursor.line);
        const end = cursor.ch;

        // Extracting the string up to the cursor to parse the context
        const stringUpToCursor = line.substring(0, end);

        function findSuggestions(obj, parts, fullPath) {
            const currentPart = parts.shift();
            const nextObject = obj[currentPart];

            if (!parts.length && nextObject) {
                // If no more parts and there's a next object, suggest methods or further properties
                let suggestions = [];
                if (nextObject._methods) {
                    suggestions = nextObject._methods.map(method => `${method}`);
                }
                suggestions.push(...Object.keys(nextObject).filter(k => !k.startsWith('_')).map(key => `${key}`));
                return suggestions;
            } else if (nextObject && parts.length > 0) {
                // More parts to process, continue recursion
                return findSuggestions(nextObject, parts, `${fullPath}.${currentPart}`);
            }
            return []; // No valid path found
        }

        // Determine where the last segment starts
        const lastDotIndex = stringUpToCursor.lastIndexOf('.');
        const prefix = stringUpToCursor.substring(0, lastDotIndex + 1); // Include the dot in prefix

        // Split the string by dots, include path parts for context
        const pathParts = stringUpToCursor.split('.').filter(Boolean);
        const suggestions = findSuggestions(modelHints, [...pathParts], pathParts.slice(0, -1).join('.')) || [];

        return {
            list: suggestions,
            from: CodeMirror.Pos(cursor.line, lastDotIndex + 1), // Start replacing right after the last dot
            to: CodeMirror.Pos(cursor.line, end)
        };
    }
})();
