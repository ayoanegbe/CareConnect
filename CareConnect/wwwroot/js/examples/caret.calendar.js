/*
Name: 			Pages / Calendar - Examples
Written by: 	Okler Themes - (http://www.okler.net)
Theme Version: 	3.1.0
*/

(function($) {

	'use strict';

	var schedules = [];

	function setupSchedules() {
		schedules = []; // clear events list

		// get all existing events from event service and map them to fullcalendar event format
		$.ajax({
			type: "GET",
			url: "/Home/GetEvents",
			success: function (data) {
				$.each(data, function () {
					schedules.push({
						title: this.title,
						start: this.startTime,
						end: this.endTime,
						color: this.color
					});
				});
				initCalendar(schedules);
			},
			error: function (error) {
				alert('failed');
			}
		});
	}

	var initCalendarDragNDrop = function() {
		var Draggable = FullCalendar.Draggable;

		$('#external-events div.external-event').each(function() {
			new Draggable($(this)[0], {
		      	itemSelector: '.external-event',
		      	eventData: function(eventEl) {
		      		var eventObj = $( eventEl ).data('event');
		        	return eventObj;
		      	}
		    });
		});
	};

	var initCalendar = function(schedules) {
		var $calendar = $('#calendar');
		var date = new Date();
		//var d = date.getDate();
		//var m = date.getMonth();
		//var y = date.getFullYear();

		var $calendarInstace = new FullCalendar.Calendar( $calendar[0], {
			themeSystem: 'bootstrap',
			eventDisplay: 'block',
			headerToolbar: {
				start: 'title',
				center: '',
				end: 'prev,today,next,dayGridDay,dayGridWeek,dayGridMonth'
			},
			bootstrapFontAwesome: {
				close: 'fa-times',
				prev: 'fa-caret-left',
				next: 'fa-caret-right',
				prevYear: 'fa-angle-double-left',
				nextYear: 'fa-angle-double-right'
			},
			editable: true,
			droppable: true, // this allows things to be dropped onto the calendar !!!
			drop: function(eventDropInfo) { // this function is called when something is dropped
				
				// is the "remove after drop" checkbox checked?
		        if ($('#RemoveAfterDrop').is(':checked')) {
		          // if so, remove the element from the "Draggable Events" list
		          eventDropInfo.draggedEl.parentNode.removeChild(eventDropInfo.draggedEl);
		        }

			},
			events: schedules

		});

		$calendarInstace.render();

		// FIX INPUTS TO BOOTSTRAP VERSIONS
		var $calendarButtons = $calendar.find('.fc-header-right > span');
		$calendarButtons
			.filter('.fc-button-prev, .fc-button-today, .fc-button-next')
				.wrapAll('<div class="btn-group mt-sm mr-md mb-sm ml-sm"></div>')
				.parent()
				.after('<br class="hidden"/>');

		$calendarButtons
			.not('.fc-button-prev, .fc-button-today, .fc-button-next')
				.wrapAll('<div class="btn-group mb-sm mt-sm"></div>');

		$calendarButtons
			.attr({ 'class': 'btn btn-sm btn-default' });
	};

	$(function () {
		setupSchedules();
		//initCalendar();
		initCalendarDragNDrop();
	});

	$('#addEventBtn').click(function () {
		openAddEditForm();
	});

	function openAddEditForm() {
		
		$('#eventInfoModal').modal('hide');
		$('#addEditEventModal').modal();
	}
}).apply(this, [jQuery]);