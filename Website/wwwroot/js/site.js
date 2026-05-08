function toggleSidebar() {
	const overlay = document.getElementById("overlay");
	overlay.classList.toggle('overlayShown');

	const sidebar = document.getElementById("sidebar");
	sidebar.classList.toggle('sidebarHidden');
}

function toggleUserInfo() {
	const userInfo = document.getElementById("user-info");
	userInfo.classList.toggle('userHidden');
}


function TogglePageView() {
	const listsView = document.getElementById("lists-view");
	const pageView = document.getElementById("page-view");

	listsView.classList.toggle("view-hidden");
    pageView.classList.toggle("view-hidden");
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


	// assemble and display the text box and submit button
	span.appendChild(input);
	span.appendChild(edit);
	textElement.replaceWith(span);

	input.focus();
	
}

function MoveListToBoard(buttonElement) {
	const listId = buttonElement.id;
	const currentBoardId = buttonElement.parentElement.id;

	const clickOffArea = buttonElement.parentElement.querySelector(".click-off-menu");
	clickOffArea.style.display = "block";

	// display the menu
	const menu = buttonElement.nextElementSibling;
	menu.style.display = 'block';

	// retrieve all boards the user has access to
	fetch(`/Boards/GetUserBoards`, {
		method: 'POST',
		headers: { 'Content-Type': 'application/json' },
	})
		.then(response => response.json())
		.then(data => {

			menu.style.opacity = 1;

			let html = "<ul>";
			data.forEach(b => {
				// add every board to the dropdown, other than the current one
				if (b.boardId != parseInt(currentBoardId)) {
					html += `<li onclick="TransferList(${listId}, ${b.boardId})">${b.boardName}</li>`;
				}
			});
			html += "</ul>";

			menu.innerHTML = html;

		})
		.catch(error => console.error('Error trying to get members! ', error));
}
// closes the menu if the user has clicked off it
function CloseTransferMenu(clickArea) {
	const menu = clickArea.nextElementSibling.nextElementSibling;
	menu.style.display = 'none';
	clickArea.style.display = 'none';
}

function TransferList(listId,boardId) {
	// call the controller to save the list to the new board
	fetch('/Lists/TransferList', {
		method: 'POST',
		headers: { 'Content-Type': 'application/json' },
		body: JSON.stringify({
			BoardId: parseInt(boardId),
			ListId: parseInt(listId)
		})
	})
		.then(response => {
			if (response.ok) {
				// reload the page to properly display the new board
				location.reload();
			}
		});
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

function DeleteListConfirmation(buttonElement) {
	const optionsContainer = buttonElement.parentElement;
	const listContainer = optionsContainer.parentElement;

	optionsContainer.remove();

	const confirmationContainer = document.createElement("div");
	confirmationContainer.className = "list-options-icons";
	confirmationContainer.style.display = "block";

	const text = document.createElement("p");
	text.innerHTML = "Delete?"

	const confirmButton = document.createElement("span");
	confirmButton.className = "delete-confirm";
	confirmButton.innerHTML = "a";

	const cancelButton = document.createElement("span");
	cancelButton.className = "delete-cancel";
	cancelButton.innerHTML = "✕";

	confirmationContainer.appendChild(text);
	confirmationContainer.appendChild(confirmButton);
	confirmationContainer.appendChild(cancelButton);
	listContainer.appendChild(confirmationContainer);

	// listener to delete the list when the user clicks confirm
	confirmButton.addEventListener("click", function () {
		DeleteList(confirmButton, buttonElement.id);
	});
	// listener to reload the page when the user clicks cancel
	cancelButton.addEventListener("click", function () {
		location.reload();
	});
}

function DeleteList(confirmButtonElement, listId) {
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
				confirmButtonElement.closest('.list-column').remove();
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



function NewBoard() {

	const allBoards = document.querySelectorAll(".my-board-button")
	const newBoardPos = allBoards.length + 1; // calculate the position for the new 

	const now = new Date().toISOString().substring(0, 10); // retrieve the current date

	// call the controller to save the new board in the database
	fetch('/Boards/CreateBoard', {
		method: 'POST',
		headers: { 'Content-Type': 'application/json' },
		body: JSON.stringify({
			Name: "My Board #" + newBoardPos,
			Position: newBoardPos,
			CreatedAt: now,
			UpdatedAt: now,
			IsShared: false
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

function EditBoardName(nameElement) {
	const boardId = nameElement.id; // retrieve board's id in the database

	const placeholderText = nameElement.innerText; // retrieve the board name

	// creating the elements
	const div = document.createElement("span");
	div.style.marginTop = "0px";
	div.style.display = "block";
	div.style.overflow = "visible";
	div.style.marginBottom = "5px";

	const input = document.createElement("input");
	input.type = "text";
	input.className = "board-edit-input";
	input.value = placeholderText;
	input.maxLength = 20;
	input.style.transform = 'translateX(-20px)';

	// set the input's width dynamically
	const updateWidth = () => {
		input.style.width = input.value.length + "ch";
	};
	updateWidth();
	input.addEventListener("input", updateWidth);

	const now = new Date().toISOString().substring(0, 10); // retrieve the current date

	// helper function for saving the board to the database
	const saveBoard = () => {
		console.log("ok");
		const boardNameContent = input.value;

		if (boardNameContent.trim() === "") return; // ensure empty board names aren't saved

		// call the controller to save the new board in the database
		fetch('/Boards/EditBoardName', {
			method: 'POST',
			headers: { 'Content-Type': 'application/json' },
			body: JSON.stringify({
				BoardId: parseInt(boardId),
				NewName: boardNameContent,
				UpdatedAt: now
			})
		})
			.then(response => {
				if (response.ok) {
					// reload the page to properly display the new board
					location.reload();
				}
			});
	}
	// listener to save the new board when the user presses enter
	input.addEventListener("keypress", function (event) {
		if (event.key === "Enter") {
			saveBoard();
		}
	});
	// listener to save the new board when the user clicks off of the input box
	input.addEventListener("blur", function () {
		saveBoard();
	});

	// place elements on the page
	div.appendChild(input);
	nameElement.replaceWith(div);
	input.focus();
	input.select();

}

function EditBoardDesc(descElement) {
	const boardId = descElement.id; // retrieve board's id in the database

	const placeholderText = descElement.innerText; // retrieve the board name

	// creating the elements
	const div = document.createElement("span");
	div.style.marginTop = "0px";
	div.style.display = "block";
	div.style.overflow = "visible";
	div.style.marginBottom = "5px";

	const input = document.createElement("input");
	input.type = "text";
	input.className = "board-edit-desc-input";
	input.value = placeholderText;
	input.maxLength = 50;
	input.style.transform = 'translateX(-20px)';

	// set the input's width dynamically
	const updateWidth = () => {
		input.style.width = input.value.length + "ch";
	};
	updateWidth();
	input.addEventListener("input", updateWidth);

	const now = new Date().toISOString().substring(0, 10); // retrieve the current date

	// helper function for saving the board to the database
	const saveBoard = () => {
		console.log("ok");
		const descContent = input.value;

		if (descContent.trim() === "") return; // ensure empty board names aren't saved

		// call the controller to save the new board in the database
		fetch('/Boards/EditBoardDesc', {
			method: 'POST',
			headers: { 'Content-Type': 'application/json' },
			body: JSON.stringify({
				BoardId: parseInt(boardId),
				NewDesc: descContent,
				UpdatedAt: now
			})
		})
			.then(response => {
				if (response.ok) {
					// reload the page to properly display the new board
					location.reload();
				}
			});
	}
	// listener to save the new board when the user presses enter
	input.addEventListener("keypress", function (event) {
		if (event.key === "Enter") {
			saveBoard();
		}
	});
	// listener to save the new board when the user clicks off of the input box
	input.addEventListener("blur", function () {
		saveBoard();
	});

	// place elements on the page
	div.appendChild(input);
	descElement.replaceWith(div);
	input.focus();
	input.select();
}


function MoveBoard(boardId, direction) {
	console.log("attempted to move board: " + boardId)
	// call the controller to save the new value in the database
	fetch('/Boards/MoveBoard', {
		method: 'POST',
		headers: { 'Content-Type': 'application/json' },
		body: JSON.stringify({
			BoardId: parseInt(boardId),
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

function OpenBoardOptionsBuffer(buttonElement, boardId, boardName, boardDesc) {
	toggleSidebar();
	OpenBoardOptions(buttonElement, boardId, boardName, boardDesc)
}
function OpenBoardOptions(buttonElement, boardId, boardName, boardDesc) {
	toggleSidebar();
	const overlay = document.getElementById("overlay");
	overlay.classList.toggle('overlayShown');

	// create the options panel elements
	const panel = document.createElement("div");
	panel.className = buttonElement.classList.contains("shared") ? "shared-board-options-panel" : "board-options-panel";
	panel.innerHTML = "<h1>Board options</h1>";

	const cancelButton = document.createElement("button");
	cancelButton.className = "board-options-cancel";
	cancelButton.innerHTML = "✕";

	const nameInput = document.createElement("input");
	nameInput.className = "board-options-input";
	nameInput.id = "board-name";
	nameInput.classList.add("name");
	nameInput.value = boardName;
	const nameLabel = document.createElement("label");
	nameLabel.htmlFor = "board-name";
	nameLabel.innerHTML = "Board name: <br/>";

	const descInput = document.createElement("textarea");
	descInput.className = "board-options-input";
	descInput.id = "board-desc";
	descInput.classList.add("desc");
	descInput.value = boardDesc;
	descInput.maxLength = 50;
	const descLabel = document.createElement("label");
	descLabel.htmlFor = "board-desc";
	descLabel.innerHTML = "Board description: <br/>";

	const saveButton = document.createElement("button");
	saveButton.className = "board-options-save";
	saveButton.innerHTML = "✔&nbsp Save changes";

	const deleteButton = document.createElement("button");
	deleteButton.className = "board-options-delete";
	deleteButton.innerHTML = "🗑&nbsp Delete board";
	let deleteConfirmation = false;

	const convertButton = document.createElement("button");
	convertButton.className = "board-options-convert";
	convertButton.innerHTML = "⇄&nbsp Convert to shared board?";

	// create share board options panel elements
	const sidePanel = document.createElement("div");
	sidePanel.className = "shared-board-options-side-panel";
	let boardMembers = [];

	const joinCodeContainer = document.createElement("div");
	joinCodeContainer.className = "join-code-container";

	let joinCode = "0000";
	const joinCodeElement = document.createElement("span");
	joinCodeElement.className = "join-code";
	joinCodeElement.innerHTML = "<p style = 'font-size: 18px;'>Join code: <p>" + joinCode;

	const refreshCodeButton = document.createElement("span");
	refreshCodeButton.className = "refresh-code-button";
	refreshCodeButton.innerText = "Re-generate";

	const leaveBoardButton = document.createElement("span");
	leaveBoardButton.className = "leave-board-button";
	leaveBoardButton.innerHTML = "↩ &nbspLeave board";


	// --- helper functions

	// helper function for the deletion confirmation
	const cancelDeleteBoardConfirmation = () => {
		deleteConfirmation = false;
		deleteButton.innerHTML = "🗑&nbsp Delete board";
		deleteButton.style.fontSize = "16px";
		deleteButton.style.backgroundColor = "#7c1c1c";
		deleteButton.style.padding = "10px";
	}

	const now = new Date().toISOString().substring(0, 10); // retrieve the current date
	// helper function for saving the board to the database
	const saveBoard = () => {
		const nameContent = nameInput.value;
		const descContent = descInput.value;

		// ensure empty values aren't saved
		if (nameContent.trim() === "") return;
		if (descContent.trim() === "") return;

		// call the controller to save the new board in the database
		fetch('/Boards/EditBoard', {
			method: 'POST',
			headers: { 'Content-Type': 'application/json' },
			body: JSON.stringify({
				BoardId: parseInt(boardId),
				NewName: nameContent,
				NewDesc: descContent,
				JoinCode: joinCode,
				UpdatedAt: now
			})
		})
			.then(response => {
				if (response.ok) {
					// reload the page to properly display the new board
					location.reload();
				}
			});
	}

	// helper function for saving the board to the database
	const deleteBoard = () => {

		// call the controller to delete the board from the database
		fetch(`/Boards/DeleteBoard`, {
			method: 'POST',
			headers: { 'Content-Type': 'application/json' },
			body: JSON.stringify({
				BoardId: parseInt(boardId)
			})
		})
			.then(response => {
				if (response.ok) {
					return response.json();
				}
				else {
					alert("Could not delete board.");
				}
			})
			.then(firstBoardId => {
				// redirect to the first board belonging to the user if they are currently on the board's page
				if (window.location.pathname == "/Boards/Index/" + boardId) {
					window.location.href = '/Boards/Index/' + firstBoardId;
				}
				else {
					window.location.reload();
				}
			})
	}

	//helper function to retrieve the join code for the shared board
	const displayJoinCode = () => {
		// retrieve the join code from the database
		fetch(`/Boards/GetJoinCode`, {
			method: 'POST',
			headers: { 'Content-Type': 'application/json' },
			body: JSON.stringify({ BoardId: parseInt(boardId) })
		})
			.then(response => response.json())
			.then(data => {
				// display the board's join code
				joinCode = data.code;
				joinCodeElement.innerHTML = "<p style = 'font-size: 18px;'>Join code: <p>" + joinCode;
			})
			.catch(error => console.error('Error trying to get code! ', error));
	}

	//helper function to retrieve all members of the shared board
	const displayBoardMembers = () => {
		// retrieve the join code from the database
		fetch(`/Boards/GetBoardMembers`, {
			method: 'POST',
			headers: { 'Content-Type': 'application/json' },
			body: JSON.stringify({ BoardId: parseInt(boardId) })
		})
			.then(response => response.json())
			.then(data => {
				// display the board's join code
				boardMembers = data.members;
				adminName = data.admin;
				userName = data.user;
				console.log("admin of board: " + adminName);
				console.log("logged in user: " + userName);

				if (adminName === userName) {
					panel.appendChild(deleteButton);
				}
				else {
					panel.classList.add("notAdmin");
					sidePanel.classList.add("notAdmin");
					joinCodeContainer.classList.add("codeNotAdmin");
				}

				// collate the html to be displayed in the panel
				let panelContent = "<h1>Board members</h1>\n<ul>";
				panelContent += `<li><b>Admin: </b>${adminName}</li>`;

				for (let memberName of boardMembers) { // display all members of the board
					if (memberName != adminName) { // only display the remove member button on members who arent the admin, and if the logged in user is the admin
						panelContent += `
						<li>
							${memberName} 
							${(userName == adminName) ? `<button class="remove-member" onclick="RemoveMember('${memberName}', ${ boardId }, this)">✕</button>` : ``}
						</li>`;
					}
				}
				sidePanel.innerHTML += "</ul>";
				sidePanel.innerHTML += panelContent;
				if (userName !== adminName) sidePanel.appendChild(leaveBoardButton);
			})
			.catch(error => console.error('Error trying to get members! ', error));
	}



	// -- appending the elements

	// append all options panel elements to page
	panel.appendChild(cancelButton);
	panel.appendChild(nameLabel);
	panel.appendChild(nameInput);
	panel.appendChild(descLabel);
	panel.appendChild(descInput);
	panel.appendChild(saveButton);


	// append the elements for individual boards ONLY
	if (!buttonElement.classList.contains("shared")) {
		panel.appendChild(deleteButton);
		panel.appendChild(convertButton);
		overlay.appendChild(panel);
	}
	// append the elements for shared boards ONLY
	else {
		overlay.appendChild(panel);
		overlay.appendChild(sidePanel);
		panel.appendChild(joinCodeContainer);
		joinCodeContainer.appendChild(joinCodeElement);
		joinCodeContainer.appendChild(refreshCodeButton);

		saveButton.style.marginTop = "130px"

		displayJoinCode();
		displayBoardMembers();
	}


	// -- listeners

	// listener to delete the list when the user clicks save
	saveButton.addEventListener("click", function () {
		saveBoard(boardId);
	});

	// listener to delete the list when the user clicks delete
	deleteButton.addEventListener("click", function () {
		console.log("delete confirmation active: " + deleteConfirmation)
		if (!deleteConfirmation) {
			deleteConfirmation = true;
			console.log("now: " + deleteConfirmation)
			deleteButton.innerHTML = "<b>⚠︎&nbsp Confirm deletion</b>";
			deleteButton.style.fontSize = "18px";
			deleteButton.style.backgroundColor = "#a81818"
			deleteButton.style.padding = "14px";

			setTimeout(cancelDeleteBoardConfirmation, 3000)
		}
		else {
			console.log("now: " + deleteConfirmation)
			deleteBoard(boardId);
		}
	});

	// listener to hide the options panels when the user clicks cancel
	cancelButton.addEventListener("click", function () {
		panel.remove();
		const sidePanelElement = overlay.querySelector(".shared-board-options-side-panel");
		if (sidePanelElement) { sidePanelElement.remove(); }

		overlay.classList.toggle('overlayShown');
	});

	// listener to convert a board to a shared board
	convertButton.addEventListener("click", function () {
		joinCode = Math.floor(1000 + Math.random() * 9000);

		// call the controller to change the board to a shared board
		fetch('/Boards/ConvertBoard', {
			method: 'POST',
			headers: { 'Content-Type': 'application/json' },
			body: JSON.stringify({
				BoardId: parseInt(boardId),
				JoinCode: parseInt(joinCode),
				UpdatedAt: now
			})
		})
			.then(response => {
				if (response.ok) {
					// reload the page to properly display the new board
					location.reload();
				}
			});
	});

	// listener to generate a new join code
	refreshCodeButton.addEventListener("click", function () {
		joinCode = Math.floor(1000 + Math.random() * 9000);
		joinCodeElement.innerHTML = "<p style = 'font-size: 18px;'>Join code: <p>" + joinCode;
	});

	// listener to leave the shared board
	leaveBoardButton.addEventListener("click", function () {
		MemberLeaveBoard(userName, boardId);
	});

}

// function to remove a member from a shared bored
function RemoveMember(memberName, boardId, buttonElement) {
	console.log("trying to remove member: " + memberName);

	// call the controller to remove the member from the board in the database
	fetch(`/Boards/RemoveMember`, {
		method: 'POST',
		headers: { 'Content-Type': 'application/json' },
		body: JSON.stringify({
			BoardId: parseInt(boardId),
			MemberName: memberName
		})
	})
		.then(response => {
			if (response.ok) {
				buttonElement.parentElement.remove();
				return;
			}
			else {
				alert("Could not remove user.");
			}
		})
}
// function to remove the current user from a shared bored
function MemberLeaveBoard(memberName, boardId) {
	console.log(memberName + " is trying to leave board " + boardId);

	fetch(`/Boards/RemoveMember`, {
		method: 'POST',
		headers: { 'Content-Type': 'application/json' },
		body: JSON.stringify({
			BoardId: parseInt(boardId),
			MemberName: memberName
		})
	})
		.then(response => {
			if (response.ok) {
				return response.json();
			}
			else {
				alert("Could not remove user.");
			}
		})
		.then(firstBoardId => {
			// redirect to the first board belonging to the user
			window.location.href = '/Boards/Index/' + firstBoardId;
		})
}

function JoinSharedBoard(buttonElement) {
	toggleSidebar();
	const overlay = document.getElementById("overlay");
	overlay.classList.toggle('overlayShown');

	// create the options panel elements
	const panel = document.createElement("div");
	panel.className = "board-join-panel";
	panel.innerHTML = "<h1>Join a board</h1>";

	const cancelButton = document.createElement("button");
	cancelButton.className = "board-options-cancel";
	cancelButton.innerHTML = "✕";

	const codeInput = document.createElement("input");
	codeInput.className = "board-code-input";
	codeInput.id = "join-code";
	codeInput.placeholder = "Code";
	codeInput.maxLength = 4;
	const codeLabel = document.createElement("label");
	codeLabel.htmlFor = "join-code";
	codeLabel.innerHTML = "Enter join code: <br/>";

	const joinButton = document.createElement("button");
	joinButton.className = "board-options-convert";
	joinButton.innerHTML = "Join";

	// append elements to the page
	panel.appendChild(cancelButton);
	panel.appendChild(codeLabel);
	panel.appendChild(codeInput);
	panel.appendChild(joinButton);
	overlay.appendChild(panel);

	codeInput.focus();

	// listener to hide the options panels when the user clicks cancel
	cancelButton.addEventListener("click", function () {
		panel.remove();
		const sidePanelElement = overlay.querySelector(".board-join-panel");
		if (sidePanelElement) { sidePanelElement.remove(); }

		overlay.classList.toggle('overlayShown');
	});

	// listener for the join button
	joinButton.addEventListener("click", function () {
		const joinCode = codeInput.value;

		// call the controller to check if a board with that code exists
		fetch('/Boards/UserJoinBoard', {
			method: 'POST',
			headers: { 'Content-Type': 'application/json' },
			body: JSON.stringify({
				JoinCode: parseInt(joinCode),
			})
		})
			.then(response => {
				if (response.ok) {
					return response.json();
				}
				else {
					alert("Could not join board.");
				}
			})
			.then(boardId => {
				// redirect to the newly joined board
				window.location.href = '/Boards/Index/' + boardId;
			});
	});
}

function NewSharedBoard(buttonElement) {
	const allSharedBoards = document.querySelectorAll(".shared-board-button")
	const newSharedBoardPos = allSharedBoards.length + 1; // calculate the position for the new 

	const now = new Date().toISOString().substring(0, 10); // retrieve the current date

	// call the controller to save the new board in the database
	fetch('/Boards/CreateBoard', {
		method: 'POST',
		headers: { 'Content-Type': 'application/json' },
		body: JSON.stringify({
			Name: "Shared Board #" + newSharedBoardPos,
			Position: newSharedBoardPos,
			CreatedAt: now,
			UpdatedAt: now,
			IsShared: true
		})
	})
		.then(response => {
			if (response.ok) {
				return response.json();
			}
		})
		.then(boardId => {
			// redirect to the new board webpage upon receiving the response from the server
			window.location.href = `/Boards/Index/${boardId}`;
		})
}


function DeleteBoard(buttonElement) {

	boardId = buttonElement.id;

	const boardInfoElement = buttonElement.previousElementSibling;
	boardInfoElement.remove();

	buttonElement.innerHTML = "•&nbsp&nbsp&nbsp&nbspDelete?"
	buttonElement.className = "delete-board-confirm"

	const confirmButton = document.createElement("button");
	confirmButton.className = "delete-confirm-yes";
	confirmButton.innerHTML = "a";

	const cancelButton = document.createElement("button");
	cancelButton.className = "delete-confirm-no";
	cancelButton.innerHTML = "✕";

	buttonElement.appendChild(confirmButton);
	buttonElement.appendChild(cancelButton);

	// listener to delete the board when the user clicks confirm
	confirmButton.addEventListener("click", function () {
		deleteBoard();
	});

	// listener to reload the page when the user clicks cancel
	cancelButton.addEventListener("click", function () {
		window.location.reload();
	});


	// helper function for saving the board to the database
	const deleteBoard = () => {

		// call the controller to delete the board from the database
		fetch(`/Boards/DeleteBoard`, {
			method: 'POST',
			headers: { 'Content-Type': 'application/json' },
			body: JSON.stringify({
				BoardId: parseInt(boardId)
			})
		})
			.then(response => {
				if (response.ok) {
					return response.json();
				}
				else {
					alert("Could not delete board.");
				}
			})
			.then(firstBoardId => {
				// redirect to the first board belonging to the user
				window.location.href = '/Boards/Index/' + firstBoardId;
			})
	}
}