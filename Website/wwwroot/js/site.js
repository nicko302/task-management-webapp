function toggleSidebar() {
    const sidebar = document.getElementById("sidebar");
    sidebar.classList.toggle('sidebarHidden');
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

	const input = document.createElement("input");
	input.type = "text";
	input.className = "task-edit-input";
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

		if (taskContent.trim() === "") return; // ensure empty tasks aren't saved

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


	// display the text box and plus sign
	li.appendChild(plus);
	li.appendChild(input);
	list.insertBefore(li, buttonElement.parentElement); // inserts text box before the new task button

	buttonElement.style.display = "none"; // hides the new task button

	input.focus(); // focuses on the text box
}


function DeleteTask(buttonElement) {
	const taskId = buttonElement.id; // retrieve task's id in the database

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
	const taskId = buttonElement.id; // retrieve task's id in the database

	const textElement = document.getElementById('label_' + taskId);
	const placeholderText = textElement.innerText; // retrieve the task's content

	const li = textElement.closest('li'); // retrieve li element
	li.style.top = "-5px";
	li.style.height = "33px";

	const span = document.createElement("span");
	span.style.marginTop = "0px";

	const edit = document.createElement("span");
	edit.className = "edit-submit";
	edit.textContent = "✎";
	edit.id = "edit-submit";

	const input = document.createElement("input");
	input.type = "text";
	input.className = "task-edit-input";
	input.value = placeholderText;
	input.style.transform = 'translateX(2px)';

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


	// assemble and display the text box and submit button
	span.appendChild(edit);
	span.appendChild(input);
	textElement.replaceWith(span);

	// hide the edit/delete buttons
	const actionButtons = li.querySelector('.edit-delete-span');
	if (actionButtons) actionButtons.style.display = 'none';

	input.focus();
}


function NewList(buttonElement) {
	const listsRow = buttonElement.parentElement; // retrieve lists-row element
	const boardId = listsRow.dataset.boardId; // retrieve the current board's ID in the database
	const listPos = document.querySelectorAll('.list-column').length + 1; // retrieve the position value of the new list
	const now = new Date().toISOString().substring(0, 10); // retrieve the current date


	// create the new dummy list for name entry
	//<div class="list-column" style="background-color: bgColour;">
	//<span class="list-name">list.Name</span>

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
		const listName = input.value;

		if (listName.trim() === "") return; // ensure empty tasks aren't saved

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


	// display the text box and plus sign
	newListDiv.appendChild(input);
	newListDiv.appendChild(plus);
	listsRow.insertBefore(newListDiv, buttonElement); // adds new list to page

	input.focus(); // focuses on the text box
}


function EditList(buttonElement) {
	const listId = buttonElement.id; // retrieve task's id in the database

	const textElement = buttonElement.previousElementSibling;
	const placeholderText = textElement.innerText; // retrieve the task's content

	// creating the elements
	const span = document.createElement("span");
	span.style.marginTop = "0px";
	span.style.display = "block";
	span.style.height = "0px";
	span.style.overflow = "visible";
	span.style.marginBottom = "5px";

	const edit = document.createElement("span");
	edit.className = "edit-submit";
	edit.textContent = "✎";
	edit.id = "edit-submit";
	edit.style.left = "40px";
	edit.style.fontSize = "25px";

	const input = document.createElement("input");
	input.type = "text";
	input.className = "list-edit-input";
	input.value = placeholderText;
	input.style.transform = 'translateX(2px)';
	input.maxLength = "14";

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

	colours.forEach(c => {
		console.log(`Comparing: ArrayItem(${c}) with Current(${currentColourText})`);
		DisplayColourOption(c, currentColourText, colourContainer);
	});


	const now = new Date().toISOString().substring(0, 10); // retrieve the current date


	// helper function for saving the task to the database
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

	// listener to save the new task when the user clicks the plus
	edit.addEventListener("click", function () {
		saveList();
	});


	// display the colour select element
	listDiv.prepend(colourContainer); // inserts colour container before the list name


	// assemble and display the text box and submit button
	span.appendChild(input);
	span.appendChild(edit);
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
			return "#edd83b";
		default:
			return "#333333";
	}
}
function DisplayColourOption(colourName, currentColourName, container) {
	const colourDot = document.createElement("span"); // Rename to avoid conflict
	colourDot.className = "colour-option";
	colourDot.style.color = TextToColour(colourName);
	colourDot.textContent = "•";

	if (colourName === currentColourName) {
		colourDot.className += " selected";
	}

	container.appendChild(colourDot);
}
