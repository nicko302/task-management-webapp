function toggleSidebar() {
	console.log("1");
	const overlay = document.getElementById("overlay");
	overlay.classList.toggle('overlayShown');

	const sidebar = document.getElementById("sidebar");
	sidebar.classList.toggle('sidebarHidden');
	console.log("2");
}

function toggleCheckbox(element) {
	const taskId = element.getAttribute('data-task-id');
	const label = document.getElementById('label_' + taskId);
	const isChecked = element.checked;

	if (label) {
		if (element.checked) {
			label.classList.add('completed-task');
			label.classList.remove('task');
		} else {
			label.classList.remove('completed-task');
			label.classList.add('task');
		}
	}

	// call the controller to update the "completed" value in the database
	fetch('/Tasks/UpdateCompletedValue', {
		method: 'POST',
		headers: {
			'Content-Type': 'application/json'
		},
		body: JSON.stringify({
			id: parseInt(taskId),
			completed: isChecked ? 1 : 0
		})
	})
}

function NewTask(buttonElement) {
	const list = buttonElement.closest('ul'); // retrieve list element
	const li = document.createElement('li'); // create new list item
	li.style.listStyle = "none";
	li.style.marginTop = "-20px";
	li.style.marginBottom = "-2px";

	const input = document.createElement("input");
	input.type = "text";
	input.className = "task-edit-input";
	input.style.marginLeft = "-3px";
	input.placeholder = "Enter new task";

	const plus = document.createElement("span");
	plus.className = "plus";
	plus.textContent = "+";
	plus.id = "plus";


	const listId = list.id; // retrieve the list's id in the database
	const taskPos = list.getElementsByTagName("li").length; // retrieve the position value of the new task
	const now = new Date().toISOString().substring(0, 10); // retrieve the current date


	// helper function for saving the task to the database
	const saveTask = () => {
		const taskContent = input.value;

		// ensure empty tasks are dealt with
		if (taskContent.trim() === "") {
			taskContent = " ";
		} 

		// call the controller to save the new task in the database
		fetch('/Tasks/CreateTask', {
			method: 'POST',
			headers: { 'Content-Type': 'application/json' },
			body: JSON.stringify({
				Content: taskContent,
				ListId: parseInt(listId),
				Position: taskPos,
				CreatedAt: now,
				UpdatedAt: now
			})
		})
			.then(response => {
				if (response.ok) {
					// reload the page to properly display the new task
					location.reload();
				}
			});
	}

	// listener to save the new task when the user presses enter
	input.addEventListener("keypress", function (event) {
		if (event.key === "Enter") {
			saveTask();
		}
	});

	// listener to save the new task when the user clicks the plus
	plus.addEventListener("click", function () {
		saveTask();
	});

	// listener to save the new task when the user clicks off of the input box
	input.addEventListener("blur", function () {
		saveTask();
	});


	// display the text box and plus sign
	li.appendChild(plus);
	li.appendChild(input);
	list.insertBefore(li, buttonElement.parentElement); // inserts text box before the new task button

	buttonElement.style.display = "none"; // hides the new task button

	input.focus(); // focuses on the text 
}


function DeleteTask(buttonElement) {
	const taskId = buttonElement.id; // retrieve task's id
	const taskPos = buttonElement.getAttribute('data-task-pos'); // retrieve task's position value
	const tasksInList = buttonElement.parentElement.parentElement.id;

	// call the controller to delete the task from the database
	fetch(`/Tasks/DeleteTask`, {
		method: 'POST',
		headers: { 'Content-Type': 'application/json' },
		body: JSON.stringify({
			TaskId: parseInt(taskId)
		})
	})
		.then(response => {
			if (response.ok) {
				// remove the <li> element
				buttonElement.closest('li').remove();
			} else {
				alert("Could not delete task.");
			}
		});
}

function EditTask(buttonElement) {
	const taskId = buttonElement.id; // retrieve task's id

	const textElement = document.getElementById('label_' + taskId);
	const placeholderText = textElement.innerText; // retrieve the task's content

	const li = textElement.closest('li'); // retrieve li element
	li.style.top = "-5px";
	li.style.height = "33px";

	const span = document.createElement("span");
	span.style.marginTop = "0px";

	const edit = document.createElement("span");
	edit.className = "edit-submit-task";
	edit.textContent = "✎";
	edit.id = "edit-submit-task";

	const input = document.createElement("input");
	input.type = "text";
	input.className = "task-edit-input";
	input.value = placeholderText;
	input.style.transform = 'translateX(2px)';

	// hide the up + down arrows
	const arrowDown = li.querySelector(".down");
	const arrowUp = li.querySelector(".up");
	arrowDown.style.display = "none";
	arrowUp.style.display = "none";

	const now = new Date().toISOString().substring(0, 10); // retrieve the current date


	// helper function for saving the task to the database
	const saveTask = () => {
		const taskContent = input.value;

		if (taskContent.trim() === "") return; // ensure empty tasks aren't saved

		// call the controller to save the new task in the database
		fetch('/Tasks/EditTask', {
			method: 'POST',
			headers: { 'Content-Type': 'application/json' },
			body: JSON.stringify({
				TaskId: parseInt(taskId),
				Content: taskContent,
				UpdatedAt: now
			})
		})
			.then(response => {
				if (response.ok) {
					// reload the page to properly display the new task
					location.reload();
				}
			});
	}

	// listener to save the new task when the user presses enter
	input.addEventListener("keypress", function (event) {
		if (event.key === "Enter") {
			saveTask();
		}
	});

	// listener to save the new task when the user clicks the plus
	edit.addEventListener("click", function () {
		saveTask();
	});

	// listener to save the new task when the user clicks off of the input box
	input.addEventListener("blur", function () {
		saveTask();
	});

	// assemble and display the text box and submit button
	span.appendChild(edit);
	span.appendChild(input);
	textElement.replaceWith(span);

	// hide the edit/delete buttons
	const actionButtons = li.querySelector('.edit-delete-span');
	if (actionButtons) actionButtons.style.display = 'none';

	input.focus();
}

function MoveTask(buttonElement, direction) {
	const taskId = buttonElement.id; // retrieve task's id

	// helper function for saving the new value to the database
	const saveTask = () => {

		// call the controller to save the new value in the database
		fetch('/Tasks/MoveTask', {
			method: 'POST',
			headers: { 'Content-Type': 'application/json' },
			body: JSON.stringify({
				TaskId: parseInt(taskId),
				Direction: direction,
			})
		})
			.then(response => {
				if (response.ok) {
					// reload the page to properly display the new task position
					location.reload();
				}
			});
	}

	saveTask();
}


function NewList(buttonElement) {
	const listsRow = buttonElement.parentElement; // retrieve lists-row element
	const boardId = listsRow.dataset.boardId; // retrieve the current board's ID in the database
	const listPos = document.querySelectorAll('.list-column').length + 1; // retrieve the position value of the new list
	const now = new Date().toISOString().substring(0, 10); // retrieve the current date

	const newListDiv = document.createElement('div'); // create new div
	newListDiv.className = "list-column";
	newListDiv.style.backgroundColor = "#103821";
	newListDiv.style.paddingTop = "20px";

	const input = document.createElement("input");
	input.type = "text";
	input.className = "list-name-edit-input";
	input.placeholder = "Enter list name";

	const plus = document.createElement("span");
	plus.className = "plus";
	plus.textContent = "+";
	plus.id = "plus";
	plus.style.left = "40px";
	plus.style.position = "relative";

	// helper function for saving the list to the database
	const saveList = () => {
		let listName = input.value;

		// ensure empty tasks are dealt with
		if (listName.trim() === "") {
			listName = "Untitled"
		}

		// call the controller to save the new task in the database
		fetch('/Lists/CreateList', {
			method: 'POST',
			headers: { 'Content-Type': 'application/json' },
			body: JSON.stringify({
				Name: listName,
				BoardId: parseInt(boardId),
				Position: listPos,
				CreatedAt: now,
				UpdatedAt: now,
				Colour: "Green"
			})
		})
			.then(response => {
				if (response.ok) {
					// reload the page to properly display the new task
					location.reload();
				}
			});
	}


	// listener to save the new list when the user presses enter
	input.addEventListener("keypress", function (event) {
		if (event.key === "Enter") {
			saveList();
		}
	});
	// listener to save the new list when the user clicks the plus
	plus.addEventListener("click", function () {
		saveList();
	});
	// listener to save the new task when the user clicks off of the input box
	input.addEventListener("blur", function () {
		saveList();
	});


	// display the text box and plus sign
	newListDiv.appendChild(input);
	newListDiv.appendChild(plus);
	listsRow.insertBefore(newListDiv, buttonElement); // adds new list to page

	input.focus(); // focuses on the text box
}


function EditList(titleElement) {
	const buttonElement = titleElement.nextElementSibling.querySelector(".colour-list");

	buttonElement.parentElement.classList.add("isEditing"); // hide the edit/delete buttons

	const listId = titleElement.id; // retrieve list's id in the database

	const textElement = buttonElement.closest(".list-column").querySelector(".list-name");
	const placeholderText = textElement.innerText; // retrieve the list's content

	// creating the elements
	const span = document.createElement("span");
	span.style.marginTop = "0px";
	span.style.display = "block";
	span.style.height = "0px";
	span.style.overflow = "visible";
	span.style.marginBottom = "5px";

	const edit = document.createElement("span");
	edit.className = "edit-submit";
	edit.textContent = "a";
	edit.id = "edit-submit";

	const deleteElement = document.createElement("span");
	deleteElement.className = "delete-list";
	deleteElement.textContent = "🗑";
	deleteElement.id = listId;

	const input = document.createElement("input");
	input.type = "text";
	input.className = "list-edit-input";
	input.value = placeholderText;
	input.style.transform = 'translateX(2px)';
	input.maxLength = "14";

	// fix positioning of other elements
	const tasks = buttonElement.parentElement.nextElementSibling;
	tasks.style.marginTop = "50px";
	const newTaskButton = tasks.querySelector("button");
	newTaskButton.style.display = "none";

	const now = new Date().toISOString().substring(0, 10); // retrieve the current date

	// helper function for saving the list to the database
	const saveList = () => {
		const listContent = input.value;

		if (listContent.trim() === "") return; // ensure empty list names aren't saved

		// call the controller to save the new list in the database
		fetch('/Lists/EditList', {
			method: 'POST',
			headers: { 'Content-Type': 'application/json' },
			body: JSON.stringify({
				ListId: parseInt(listId),
				NewName: listContent,
				UpdatedAt: now
			})
		})
			.then(response => {
				if (response.ok) {
					// reload the page to properly display the new list
					location.reload();
				}
			});
	}

	// listener to save the new list when the user presses enter
	input.addEventListener("keypress", function (event) {
		if (event.key === "Enter") {
			saveList();
		}
	});

	// listener to save the new list when the user clicks the plus
	edit.addEventListener("click", function () {
		saveList();
	});

	// listener to save the new list when the user clicks off of the input box
	input.addEventListener("blur", function () {
		saveList();
	});

	// listener to delete the list when the user clicks the delete button
	deleteElement.addEventListener("click", function () {
		DeleteList(deleteElement);
	});


	// assemble and display the text box and submit button
	span.appendChild(input);
	span.appendChild(edit);
	span.appendChild(deleteElement);
	textElement.replaceWith(span);

	input.focus();
	
}

// helper function for returning the hex equivalent of a colour name as text
function TextToColour(text) {
	switch (text?.toLowerCase()) {
		case "red":
			return "#451b12";
		case "green":
			return "#103821";
		case "blue":
			return "#0d365c";
		case "orange":
			return "#a1620b";
		case "purple":
			return "#4d2d47";
		case "teal":
			return "#1d524b";
		case "brown":
			return "#3d332a";
		case "yellow":
			return "#bdad39";
		default:
			return "#333333";
	}
}

function EditListColour(buttonElement) {
	buttonElement.parentElement.classList.add("isEditing"); // hide the edit/delete buttons

	const listId = buttonElement.id; // retrieve list's id in the database

	// retrieve current colour
	const colours = ["red", "green", "blue", "orange", "purple", "teal", "brown", "yellow"];
	const listDiv = buttonElement.closest(".list-column");
	const currentColourHex = listDiv.getAttribute('data-colour-hex');
	const currentColourText = listDiv.getAttribute('data-colour-text').toLowerCase();

	console.log("Hex from HTML:", currentColourHex);
	console.log("Translated Text:", currentColourText);

	// create colour edit element
	const colourContainer = document.createElement("span");
	colourContainer.className = "colour-container";
	colourContainer.id = listId;

	colours.forEach(c => {
		DisplayColourOption(c, currentColourText, colourContainer);
	});

	// display the colour select element
	listDiv.prepend(colourContainer); // inserts colour container before the list name

}
function DisplayColourOption(colourName, currentColourName, container) {
	const colourDot = document.createElement("span"); // Rename to avoid conflict
	colourDot.className = "colour-option";
	colourDot.style.color = TextToColour(colourName);
	colourDot.textContent = "•";
	colourDot.id = colourName;
	colourDot.onclick = function () {
		ChangeListColour(this);
	};

	if (colourName === currentColourName) {
		colourDot.className += " selected";
	}

	container.appendChild(colourDot);
}

function ChangeListColour(colourDotElement) {
	let colourName = colourDotElement.id; // retrieve the name of the colour to be changed to
	colourName = String(colourName).charAt(0).toUpperCase() + String(colourName).slice(1); // make the first letter uppercase

	const listId = colourDotElement.closest(".colour-container").id; // retrieve list's id in the database

	const now = new Date().toISOString().substring(0, 10); // retrieve the current date

	// helper function for saving the list to the database
	const saveList = () => {
		// call the controller to save the new colour in the database
		fetch('/Lists/EditListColour', {
			method: 'POST',
			headers: { 'Content-Type': 'application/json' },
			body: JSON.stringify({
				ListId: parseInt(listId),
				NewColour: colourName,
				UpdatedAt: now
			})
		})
			.then(response => {
				if (response.ok) {
					// reload the page to properly display the new colour
					location.reload();
				}
			});
	}

	saveList();
}

function DeleteList(buttonElement) {
	const listId = buttonElement.id; // retrieve list's's id in the database
	// call the controller to delete the list from the database
	fetch(`/Lists/DeleteList`, {
		method: 'POST',
		headers: { 'Content-Type': 'application/json' },
		body: JSON.stringify({
			ListId: parseInt(listId)
		})
	})
		.then(response => {
			if (response.ok) {
				// remove the element
				buttonElement.closest('.list-column').remove();
			} else {
				alert("Could not delete list.");
			}
		});
}


function MoveList(buttonElement, direction) {
	const listId = buttonElement.id; // retrieve list's id

	// helper function for saving the new value to the database
	const saveList = () => {

		// call the controller to save the new value in the database
		fetch('/Lists/MoveList', {
			method: 'POST',
			headers: { 'Content-Type': 'application/json' },
			body: JSON.stringify({
				ListId: parseInt(listId),
				Direction: direction,
			})
		})
			.then(response => {
				if (response.ok) {
					// reload the page to properly display the new list position
					location.reload();
				}
			});
	}

	saveList();
}



function NewBoard(buttonElement) {

	const allBoards = document.querySelectorAll(".board-button")
	const newBoardPos = allBoards.length + 1; // calculate the position for the new 

	const now = new Date().toISOString().substring(0, 10); // retrieve the current date

	// call the controller to save the new board in the database
	fetch('/Boards/CreateBoard', {
		method: 'POST',
		headers: { 'Content-Type': 'application/json' },
		body: JSON.stringify({
			Name: "New board",
			Position: newBoardPos,
			CreatedAt: now,
			UpdatedAt: now
		})
	})
		.then(response => {
			if (response.ok) {
				return response.json();
			}
		})
		.then(boardId => {
			// redirect to the new board webpage upon receiving the response from the server
			window.location.href = '/Boards/Index/' + boardId;
		})
}