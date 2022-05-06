import React, { useEffect } from 'react';
import { useState } from 'react'
import CloseIcon from '@material-ui/icons/Close';
import TrashIcon from '@material-ui/icons/Delete';
import DoneAllIcon from '@material-ui/icons/DoneAll';
import UndoIcon from '@material-ui/icons/Undo';
import ArchiveIcon from '@material-ui/icons/Archive';

//library stuf: https://reactjsexample.com/simple-and-flexible-events-calendar-written-in-react/
import {
  MonthlyBody,
  MonthlyCalendar,
  MonthlyNav,
  DefaultMonthlyEventItem
} from '@zach.codes/react-calendar';
import { startOfMonth, subHours } from 'date-fns';
import "../../assets/css/calendar-tailwind.css";
import "../../assets/css/calendar-custom.css"; //custom css to reduce some text sizes

import { Button } from '@material-ui/core';
import { Dialog } from '@material-ui/core';
import { DialogTitle } from '@material-ui/core';
import { DialogContent } from '@material-ui/core';
import { DialogContentText } from '@material-ui/core';
import { TextField } from '@material-ui/core';
import { DialogActions } from '@material-ui/core';

import AddIcon from '@material-ui/icons/Add';
import { IconButton } from '@material-ui/core';
import { Snackbar } from '@material-ui/core';
import axios from 'axios';
import useToken from '../../custom-hooks/useToken';

// @material-ui/core components
import withStyles from "@material-ui/core/styles/withStyles";

import Table from "@material-ui/core/Table";
import TableHead from "@material-ui/core/TableHead";
import TableRow from "@material-ui/core/TableRow";
import TableBody from "@material-ui/core/TableBody";
import TableCell from "@material-ui/core/TableCell";
import { createTrue } from 'typescript';

export const MyMonthlyCalendar = () => {
  //DATA
  let [currentMonth, setCurrentMonth] = useState<Date>(startOfMonth(new Date()));
  let [myEvents, setMyEvent] = useState([{ id: "", completed: false, archived: false, title: '', date: subHours(new Date(), 2) }]);
  let [myArchivedEvents, setMyArchivedEvent] = useState([{ id: "", completed: false, archived: false, title: '', date: subHours(new Date(), 2) }]);
  let [toUpdateTask, setToUpdateTask] = useState({ id: "undefined", TaskTitle: "undefined", Description: "undefined", StartDate: new Date(), DueDate: new Date(), Completed: false, RepeatingInterval: 0, Archived: false, assignedTasks: [] })
  let [formUpdateTask, setformUpdateTask] = useState({ id: "undefined", TaskTitle: "undefined", Description: "undefined", StartDate: new Date(), DueDate: new Date(), Completed: false, RepeatingInterval: 0, Archived: false, assignedTasks: [] })
  let [toDeleteId, setToDeleteId] = useState({ id: "undefined" })

  //DIALOGS
  let [open, setOpen] = React.useState(false);
  let [confirmOpen, setConfirmOpen] = React.useState(false);
  let [confirmArchiveOpen, setConfirmArchiveOpen] = React.useState(false);
  let [openArchive, setArchiveOpen] = React.useState(false);
  let [openDeleteConfirm, setDeleteConfirmOpen] = React.useState(false);
  let [openUpdateForm, setUpdateFormOpen] = React.useState(false)

  //SNACKBARS
  let [snackbarOpen, setSnackbarOpen] = React.useState(false);
  let [snackbarOpen2, setSnackbarOpen2] = React.useState(false);

  //AUTHORIZATION
  let { userDetail, setToken } = useToken();
  let [user, setDetail] = useState(userDetail ? userDetail : { firstname: 'undefined', roles: [], lastname: 'undefined', username: 'undefined', userref: 'undefined', token: 'undefined' });



  var newTask = {
    id: "",
    completed: false,
    title: "",
    date: new Date(),
  };


  //componentDidMount etc...
  useEffect(() => getAllEvents(), []);

  function getAllEvents() {
    fetch(
      `https://localhost:44320/api/taskitems`, {
      method: 'GET',
      headers: {
        'Authorization': `Bearer ${user.token}`,
        'Content-Type': 'application/json',
      }
    }).then(result => result.json())
      .then(
        (result) => {
          var taksItems: any[] = [];
          var archivedTaksItems: any[] = [];
          for (var item of result) {
            var task = {
              id: item.id,
              completed: item.completed,
              title: item.taskTitle,
              date: new Date(item.startDate),
              archived: item.archived
            };
            if (task.archived) {
              archivedTaksItems.push(task);
            }
            taksItems.push(task);
          }
          setMyEvent(taksItems);
          setMyArchivedEvent(archivedTaksItems)
        })
  }


  function toDigitalClockString(date: Date) {
    /**
     * returns the digital date of a given date. (e.g: 10:00)
     */
    var houres; var minutes;
    if (date.getHours() < 9) {
      houres = "0" + date.getHours();
    } else {
      houres = date.getHours();
    }
    if (date.getMinutes() < 9) {
      minutes = "0" + date.getMinutes();
    } else {
      minutes = date.getMinutes();
    }
    return houres + ":" + minutes;
  }

  function toDateString(date: Date) {
    return date.getDate() + "/" + date.getMonth() + "/" + date.getFullYear()
  }

  //create task Handlers
  const handleClickOpen = () => {
    setOpen(true);
  };
  const handleClose = () => {
    setOpen(false);
  };
  const handleSubmit = () => {
    setOpen(false);


    var toSendTask = {
      taskTitle: newTask.title,
      description: "geen descriptie",
      startDate: correctDateForBackend(newTask.date),
      dueDate: new Date(),
      completed: newTask.completed,
      repeatingIntervalDays: 0,
      assignedTasks: null,
      archive: false
    }
    axios.post("https://localhost:44320/api/taskitems", toSendTask, {
      headers: {
        'method': 'POST',
        'Authorization': `Bearer ${user.token}`,
        'Content-Type': 'application/json',
      }
    })
      .then(result => getAllEvents());
  }

  function correctDateForBackend(date: Date) {
    let correctHoures = date.getHours() + 2
    let correctDate = new Date(date.setHours(correctHoures))
    return correctDate
  }


  //confirm - archive task - Handlers
  const handleClickOpenConfirm = () => {
    setConfirmOpen(true);
  };
  const handleCloseConfirm = () => {
    setConfirmOpen(false);
  };
  const open_archive = () => {
    setArchiveOpen(true)
  }
  const close_archive = () => {
    setArchiveOpen(false)
  }


  const archiveEvents = () => {
    handleCloseConfirm();
    myEvents.forEach(element => {
      if (element.completed) {

        let updateTask = {
          id: element.id,
          TaskTitle: element.title,
          Description: "undefined",
          StartDate: element.date,
          DueDate: new Date(),
          Completed: element.completed,
          RepeatingInterval: 0,
          Archived: true,
          assignedTasks: []
        }

        axios.put(`https://localhost:44320/api/taskitems/${updateTask.id}`, updateTask, {
          headers: {
            'method': 'PUT',
            'Authorization': `Bearer ${user.token}`,
            'Content-Type': 'application/json',
          }
        }).then(res => {
          getAllEvents()
        })
      }
    });
    openSnackbar();
  }

  function startArchive(item: any) {
    setToUpdateTask(
      {
        id: item.id,
        TaskTitle: item.title,
        Description: "undefined",
        StartDate: item.date,
        DueDate: new Date(),
        Completed: item.completed,
        RepeatingInterval: 0,
        Archived: true,
        assignedTasks: []
      })
    setConfirmArchiveOpen(true)
  }


  const toggleComplete = (element: { id: string; completed: boolean; title: string; date: Date; } & { date: Date; }) => {
    var index = myEvents.findIndex(value => value.id == element.id);
    if (index != -1) {
      if (myEvents[index].completed) {
        myEvents[index].completed = false;
      } else {
        myEvents[index].completed = true;
      }

      var to_update_task = {
        id: myEvents[index].id,
        TaskTitle: myEvents[index].title,
        Description: "undefined",
        StartDate: correctDateForBackend(myEvents[index].date),
        DueDate: new Date(),
        Completed: myEvents[index].completed,
        RepeatingInterval: 0,
        Archived: myEvents[index].archived,
        assignedTasks: []
      }

      axios.put(`https://localhost:44320/api/taskitems/${to_update_task.id}`, to_update_task, {
        headers: {
          'method': 'PUT',
          'Authorization': `Bearer ${user.token}`,
          'Content-Type': 'application/json',
        }
      }).then(
        (result) => {
          getAllEvents()
          openSnackbar2()
          setConfirmArchiveOpen(false);
        })
    }
  }


  //snackbar Handlers
  const openSnackbar = () => {
    setSnackbarOpen(true);
  };
  const closeSnackbar = (event: React.SyntheticEvent | React.MouseEvent, reason?: string) => {
    if (reason === 'clickaway') {
      return;
    }
    setSnackbarOpen(false);
  };
  const openSnackbar2 = () => {
    setSnackbarOpen2(true);
  };
  const closeSnackbar2 = (event: React.SyntheticEvent | React.MouseEvent, reason?: string) => {
    if (reason === 'clickaway') {
      return;
    }
    setSnackbarOpen2(false);
  };

  //form Handlers
  const handleChange = (e: string) => {
    newTask.title = e;
  }
  const handleDate = (e: string) => {
    var convertedDate = new Date(e);
    newTask.date = convertedDate;
  }
  const handleUpdateChange = (title: string) => {
    let newFormUpdateTask = { ...formUpdateTask };
    newFormUpdateTask.TaskTitle = title;
    setformUpdateTask(newFormUpdateTask)
  }
  const handleUpdateDate = (date: string) => {
    let newFormUpdateTask = { ...formUpdateTask };
    var convertedDate = new Date(date);
    newFormUpdateTask.StartDate = convertedDate;
    setformUpdateTask(newFormUpdateTask)
  }


  function Icon(props: any) {
    const completed = props.item.completed;
    const item = props.item;
    if (completed) {
      return <DoneAllIcon className="icon" onClick={() => toggleComplete(item)}></DoneAllIcon>
    }
    return <DoneAllIcon className="opacityIcon" onClick={() => toggleComplete(item)}></DoneAllIcon>
  }

  function checkAuthorization() {
    let authorized = false;
    user.roles.forEach(role => {
      if (role.toLowerCase() == "agendabeheerder" || role.toLowerCase() == "baas") {
        authorized = true;
      }
    })
    return authorized;
  }

  function NotArchived(props: any) {
    const item = props.item;
    const archived = props.item.archived;
    const index = props.index;

    const authorized = checkAuthorization()

    if (!archived && authorized) {
      return <div className={item.completed ? 'completed' : 'no-completed'}>
        <div onClick={() => openDetails(item)}>
          <DefaultMonthlyEventItem
            key={index}
            title={item.title}
            date={toDigitalClockString(item.date)}
          />
        </div>
        <div>
          <Icon item={item} ></Icon>
          <ArchiveIcon color="secondary" className="archive icon" onClick={() => startArchive(item)} />
        </div>
      </div>
    } else if (!archived) {
      return <div className={item.completed ? 'completed' : 'no-completed'}>
        <div>
          <DefaultMonthlyEventItem
            key={index}
            title={item.title}
            date={toDigitalClockString(item.date)}
          />
        </div>
        <div>
          <Icon item={item} ></Icon>
        </div>
      </div>
    }
  return <span></span>
}

function update() {
  let task = {
    id: toUpdateTask.id,
    TaskTitle: toUpdateTask.TaskTitle,
    Description: toUpdateTask.Description,
    StartDate: correctDateForBackend(toUpdateTask.StartDate),
    DueDate: toUpdateTask.DueDate,
    Completed: toUpdateTask.Completed,
    RepeatingInterval: toUpdateTask.RepeatingInterval,
    Archived: toUpdateTask.Archived,
    assignedTasks: toUpdateTask.assignedTasks,
  }
  axios.put(`https://localhost:44320/api/taskitems/${task.id}`, task, {
    headers: {
      'method': 'PUT',
      'Authorization': `Bearer ${user.token}`,
      'Content-Type': 'application/json',
    }
  }).then(
    (result) => {
      getAllEvents()
      openSnackbar()
      setConfirmArchiveOpen(false);
    })
}

function openDetails(task: any) {
  setformUpdateTask({
    id: task.id,
    TaskTitle: task.title,
    Description: "undefined",
    StartDate: task.date,
    DueDate: new Date(),
    Completed: task.completed,
    Archived: task.archived,
    RepeatingInterval: 0,
    assignedTasks: []
  })
  setUpdateFormOpen(true)
}

function toDatePicker(date: Date) {
  let formattedDate = date.getUTCFullYear()
    + "-" + ("0" + (date.getMonth() + 1)).slice(-2)
    + "-" + ("0" + date.getDate()).slice(-2)
    + "T" + ("0" + date.getHours()).slice(-2)
    + ":" + ("0" + date.getMinutes()).slice(-2)
  return formattedDate
}


function startDelete(receivedId: string) {
  setToDeleteId({ id: receivedId })
  setDeleteConfirmOpen(true)
}

function deleteTask() {
  let id = toDeleteId.id;
  axios.delete(`https://localhost:44320/api/taskitems/${id}`, {
    headers: {
      'method': 'DELETE',
      'Authorization': `Bearer ${user.token}`,
      'Content-Type': 'application/json',
    }
  })
    .then(
      result => getAllEvents()
    );
  setDeleteConfirmOpen(false)
  toDeleteId = { id: "undefined" }
  setSnackbarOpen2(true)
}

function doUpdate() {
  var updateTask = {
    id: formUpdateTask.id,
    taskTitle: formUpdateTask.TaskTitle,
    description: formUpdateTask.Description,
    startDate: correctDateForBackend(formUpdateTask.StartDate),
    dueDate: formUpdateTask.DueDate,
    completed: formUpdateTask.Completed,
    repeatingIntervalDays: formUpdateTask.RepeatingInterval,
    assignedTasks: formUpdateTask.assignedTasks,
    archive: formUpdateTask.Archived
  }

  axios.put(`https://localhost:44320/api/taskitems/${updateTask.id}`, updateTask, {
    headers: {
      'method': 'PUT',
      'Authorization': `Bearer ${user.token}`,
      'Content-Type': 'application/json',
    }
  }).then(
    (result) => {
      getAllEvents()
      openSnackbar2()
      setUpdateFormOpen(false)
    })
}
function RenderControls() {
  let authorized = checkAuthorization()
  if (authorized) {
    return (<div>
      <Button onClick={handleClickOpen} variant="outlined" color="primary"><AddIcon />Toevoegen</Button>
                  &nbsp;&nbsp;&nbsp;
      <Button onClick={handleClickOpenConfirm} variant="outlined" color="primary" ><AddIcon />Archiveren</Button>
      <Button onClick={open_archive} variant="outlined" color="secondary" className="right"><ArchiveIcon fontSize="small" /> Archief</Button>
      <div className="agendaSpace"></div>
    </div>)
  } else {
    return <div className="agendaSpace"></div>
  }
}

return (
  <div>
    <RenderControls></RenderControls>

    {/* DIALOG 1 : TAKEN ARCHIVEREN - CONFIRM*/}
    <Dialog open={confirmOpen} onClose={() => setConfirmOpen(false)} aria-labelledby="form-dialog-title">
      <DialogTitle id="form-dialog-title">Archiveren</DialogTitle>
      <DialogContent>
        <DialogContentText>
          Bent u zeker dat u alle voltooide taken wil archiveren? Andere willen de voltooide status misschien nog bekijken.
          </DialogContentText>
      </DialogContent>
      <DialogActions>
        <Button onClick={handleCloseConfirm} color="primary">
          Annuleren
          </Button>
        <Button onClick={archiveEvents} color="primary">
          Archiveren
          </Button>
      </DialogActions>
    </Dialog>

    {/* DIALOG 2 : TAAK AANMAKEN*/}
    <Dialog open={open} onClose={handleClose} aria-labelledby="form-dialog-title">
      <DialogTitle id="form-dialog-title">Taak toevoegen</DialogTitle>
      <DialogContent>
        <TextField
          autoFocus
          margin="dense"
          id="title"
          label="Taak titel"
          fullWidth
          onChange={(e) => { handleChange(e.target.value) }}
        />
        <TextField
          id="date"
          label="Datum"
          type="datetime-local"
          defaultValue="2021-04-24"
          fullWidth
          InputLabelProps={{
            shrink: true,
          }}
          onChange={(e) => { handleDate(e.target.value) }}
        />
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose} color="primary">
          Annuleren
          </Button>
        <Button onClick={() => handleSubmit()} color="primary">
          Aanmaken
          </Button>
      </DialogActions>
    </Dialog>


    {/* // DIALOG 3 : TAAK ARCHIVEREN - CONFIRM */}
    <Dialog open={confirmArchiveOpen} onClose={() => setConfirmArchiveOpen(false)} aria-labelledby="form-dialog-title">
      <DialogTitle id="form-dialog-title">Archiveren</DialogTitle>
      <DialogContent>
        <DialogContentText>
          Bent u zeker dat u deze taak wil archiveren? Andere willen de voltooide status misschien nog eens bekijken.
           </DialogContentText>
      </DialogContent>
      <DialogActions>
        <Button onClick={() => setConfirmArchiveOpen(false)} color="primary">
          Annuleren
           </Button>
        <Button onClick={() => update()} color="primary">
          Archiveren
           </Button>
      </DialogActions>
    </Dialog>

    {/* // DIALOG 4 : ARCHIEF */}
    <Dialog open={openArchive} onClose={close_archive} aria-labelledby="form-dialog-title">
      <DialogContent>
        <Table aria-label="a dense table">
          <TableHead>
            <TableRow>
              <TableCell colSpan={4} align="center">Voltooide taken</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {myArchivedEvents.map((prop, key) => {
              return (<TableRow key={key}>
                <TableCell align="right">{prop.title}</TableCell>
                <TableCell align="right">{toDigitalClockString(prop.date)}</TableCell>
                <TableCell align="right">{toDateString(prop.date)}</TableCell>
                <TableCell>
                  <DialogActions>
                    <TrashIcon onClick={() => startDelete(prop.id)} color="secondary" className="icon"></TrashIcon>
                  </DialogActions>
                </TableCell>
              </TableRow>);
            })}
          </TableBody>
        </Table>
      </DialogContent>
    </Dialog>

    {/* // DIALOG 5 : TAAK VERWIJDEREN - CONFIRM */}
    <Dialog open={openDeleteConfirm} onClose={() => setDeleteConfirmOpen(false)} aria-labelledby="form-dialog-title">
      <DialogTitle id="form-dialog-title">Verwijderen</DialogTitle>
      <DialogContent>
        <DialogContentText>
          Bent u zeker dat u deze taak definitief wilt verwijderen?
           </DialogContentText>
      </DialogContent>
      <DialogActions>
        <Button onClick={() => setDeleteConfirmOpen(false)} color="primary">
          Annuleren
           </Button>
        <Button onClick={() => deleteTask()} color="primary">
          Verwijderen
           </Button>
      </DialogActions>
    </Dialog>

    {/* DIALOG 6 : TAAK WIJZIGEN*/}
    <Dialog open={openUpdateForm} onClose={() => setUpdateFormOpen(false)} aria-labelledby="form-dialog-title">
      <DialogTitle id="form-dialog-title">Taak wijzigen</DialogTitle>
      <DialogContent>
        <TextField
          autoFocus
          margin="dense"
          id="title"
          label="Taak titel"
          fullWidth
          value={formUpdateTask.TaskTitle}
          onChange={(e) => { handleUpdateChange(e.target.value) }}
        />
        <TextField
          id="date"
          label="Datum"
          type="datetime-local"
          fullWidth
          value={toDatePicker(formUpdateTask.StartDate)}
          InputLabelProps={{
            shrink: true,
          }}
          onChange={(e) => { handleUpdateDate(e.target.value) }}
        />
      </DialogContent>
      <DialogActions>
        <Button onClick={() => setUpdateFormOpen(false)} color="primary">
          Annuleren
          </Button>
        <Button onClick={() => doUpdate()} color="primary">
          Wijzigen
          </Button>
      </DialogActions>
    </Dialog>

    {/*snackbar 1 */}
    <div>
      <Snackbar
        anchorOrigin={{
          vertical: 'bottom',
          horizontal: 'left',
        }}
        open={snackbarOpen}
        autoHideDuration={4000}
        onClose={closeSnackbar}
        message="gearchiveerd!"
        action={
          <React.Fragment>
            <IconButton aria-label="close" color="inherit" onClick={closeSnackbar}>
              <CloseIcon fontSize="small" />
            </IconButton>
          </React.Fragment>
        }
      />
    </div>


    {/*snackbar 2 */}
    <div>
      <Snackbar
        anchorOrigin={{
          vertical: 'bottom',
          horizontal: 'left',
        }}
        open={snackbarOpen2}
        autoHideDuration={2000}
        onClose={closeSnackbar2}
        message="Taak geüpdatet"
        action={
          <React.Fragment>
            <IconButton aria-label="close" color="inherit" onClick={closeSnackbar2}>
              <CloseIcon fontSize="small" />
            </IconButton>
          </React.Fragment>
        }
      />
    </div>

    <MonthlyCalendar
      currentMonth={currentMonth}
      onCurrentMonthChange={date => setCurrentMonth(date)}
    >
      <MonthlyNav />
      <MonthlyBody
        events={myEvents}
        renderDay={data =>
          data.map((item, index) => (
            <div>
              <NotArchived item={item} index={index}></NotArchived>
            </div>
          ))
        }
      />
    </MonthlyCalendar>
  </div>

);
};