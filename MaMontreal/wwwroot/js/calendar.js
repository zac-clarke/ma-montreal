
// In case there's an error when reading from the file, we will delete the file. Forcing it to recreate it
window.addEventListener('error', function (e) {
    document.getElementById('errorBox').classList.remove('d-none');
    $.ajax({
        type: "DELETE",
        url: "/Meetings/Calandar",
    })
});

const loadCalender = (data) => {
    console.log("here")
    const calendarEl = document.getElementById('calendar')
    const events = data
    //const events = [
    //  {title: 'Test', startRecur:'2023-02-03', startTime: '16:20:00', daysOfWeek: ['2']},
    //  {title: 'Test2', start:'2023-02-03T04:00:00'}
    //]
    const calendar = new FullCalendar.Calendar(calendarEl, {
        headerToolbar: {
            left: 'dayGridMonth,timeGridWeek,listWeek',
            center: 'title',
            right: 'prev,next today',
        },
        themeSystem: 'bootstrap5',
        initialView: 'listWeek',
        events: events,
        eventClick: function (info) {
            const id = info.event._def.publicId
            //TODO: Change URL to go to non - admin meeting detail page
            window.location.href = "/Manage/Meetings/Details/" + id
        }
    })
    calendar.render()
    calendar.setOption('locale', 'en-CA');
}