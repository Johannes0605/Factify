// Shared JavaScript for Quiz Create and Edit forms

let questionIndex = 0;
let optionIndexes = {};

function initializeQuestionIndex(count) {
    questionIndex = count || 0;
}

function initializeOptionIndexes(indexes) {
    optionIndexes = indexes || {};
}

function addQuestion() {
    const container = document.getElementById('questionsContainer');
    const questionDiv = document.createElement('div');
    questionDiv.className = 'card mb-3 question-card';
    questionDiv.id = `question-${questionIndex}`;
    questionDiv.innerHTML = `
        <div class="card-header bg-light d-flex justify-content-between align-items-center">
            <h6 class="mb-0">Question ${questionIndex + 1}</h6>
            <button type="button" class="btn btn-danger btn-sm" onclick="removeQuestion(${questionIndex})">
                Remove
            </button>
        </div>
        <div class="card-body">
            <div class="form-group mb-3">
                <label class="form-label">Question Text</label>
                <input type="text" name="Questions[${questionIndex}].QuestionText" class="form-control" required />
            </div>
            
            <div class="mb-2">
                <strong>Answer Options:</strong>
            </div>
            
            <div id="optionsContainer-${questionIndex}" class="ms-3">
            </div>
            
            <button type="button" class="btn btn-sm btn-outline-primary mt-2" onclick="addOption(${questionIndex})">
                Add Option
            </button>
        </div>
    `;
    container.appendChild(questionDiv);
    
    // Add 2 default options
    addOption(questionIndex);
    addOption(questionIndex);
    
    questionIndex++;
}

function removeQuestion(index) {
    const questionDiv = document.getElementById(`question-${index}`);
    if (questionDiv) {
        questionDiv.remove();
    }
}

function addOption(questionIndex) {
    if (!optionIndexes[questionIndex]) {
        optionIndexes[questionIndex] = 0;
    }
    
    const optionIndex = optionIndexes[questionIndex];
    const container = document.getElementById(`optionsContainer-${questionIndex}`);
    const optionDiv = document.createElement('div');
    optionDiv.className = 'row mb-2 align-items-center option-row';
    optionDiv.id = `question-${questionIndex}-option-${optionIndex}`;
    optionDiv.innerHTML = `
        <div class="col-md-1">
            <div class="form-check">
                <input type="checkbox" name="Questions[${questionIndex}].Options[${optionIndex}].IsCorrect" 
                       value="true" class="form-check-input" id="q${questionIndex}opt${optionIndex}">
                <input type="hidden" name="Questions[${questionIndex}].Options[${optionIndex}].IsCorrect" value="false">
                <label class="form-check-label small" for="q${questionIndex}opt${optionIndex}">Correct</label>
            </div>
        </div>
        <div class="col-md-9">
            <input type="text" name="Questions[${questionIndex}].Options[${optionIndex}].Text" 
                   class="form-control form-control-sm" placeholder="Enter option text" required />
        </div>
        <div class="col-md-2">
            <button type="button" class="btn btn-sm btn-danger" onclick="removeOption(${questionIndex}, ${optionIndex})">
                Remove
            </button>
        </div>
    `;
    container.appendChild(optionDiv);
    optionIndexes[questionIndex]++;
}

function removeOption(questionIndex, optionIndex) {
    const optionDiv = document.getElementById(`question-${questionIndex}-option-${optionIndex}`);
    if (optionDiv) {
        optionDiv.remove();
    }
}
