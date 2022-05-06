/* eslint-disable */
import React from 'react';
import 'perfect-scrollbar/css/perfect-scrollbar.css';
// @material-ui/core components
import withStyles from '@material-ui/core/styles/withStyles';
import tableStyle from '../../assets/jss/material-dashboard-react/components/tableStyle';
// core components
import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import DialogContentText from "@material-ui/core/DialogContentText";
import DialogTitle from "@material-ui/core/DialogTitle";
import Button from "@material-ui/core/Button";
import FormControl from "@material-ui/core/FormControl";
import { doJsonPostApiCall } from "../../utils/apicommunications";
import { FormHelperText, Input, InputLabel, NativeSelect } from "@material-ui/core";

interface Props {
  classes: any;
  open: boolean;
  onClose: () => void;
  userAdd: (id: string, firstName: string, lastName: string, email: string, role: string) => void;
  token: string;
}

interface State {
  firstName: string;
  lastName: string;
  email: string;
  role: string;
  password: string;
  confirmPassword: string;
  user: {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    role: string;
  };
}

class UserAddDialog extends React.Component<Props, State> {
  refs: any;
  constructor(props: Props) {
    super(props);
    this.state = {
      firstName: '',
      lastName: '',
      email: '',
      role: 'ADMINISTRATOR',
      password: '',
      confirmPassword: '',
      user: {
        id: '0000',
        firstName: 'Wouter',
        lastName: 'Pardon',
        email: 'wouter.pardon@live.be',
        role: 'Administrator'
      },
    };
  }

  handleSave = async () => {
    doJsonPostApiCall('Accounts/Register', this.props.token, {
      firstname: this.state.firstName.substring(0, 1).toUpperCase() + this.state.firstName.substring(1),
      lastname: this.state.lastName.substring(0, 1).toUpperCase() + this.state.lastName.substring(1),
      email: this.state.email,
      role: this.state.role,
      password: this.state.password,
      confirmpassword: this.state.confirmPassword,
      // token: this.props.token,
    }, (data: any) => {
      this.props.userAdd(data.id, data.firstName, data.lastName, data.email, data.role);
    }, false, (e: any) => {
      this.props.userAdd('000000', this.state.firstName, this.state.lastName, this.state.email, this.state.role);
    }
    );
    this.props.onClose();
  }

  handleClose = () => {
    this.props.onClose();
  }

  render() {
    const { classes, ...rest } = this.props;

    return (
      <Dialog
        open={this.props.open}
        onClose={this.handleClose}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
      >
        <DialogTitle id="alert-dialog-title">{'Gebruiker toevoegen'}</DialogTitle>
        <DialogContent>
          <DialogContentText id="alert-dialog-description">

            <FormControl className={classes.margin}>
              <InputLabel htmlFor="firstNameInput">Voornaam:</InputLabel>
              <Input id="firstNameInput" error={this.state.firstName.length < 1} required={true} aria-describedby="fn-error" type="text" onChange={(e) => { this.setState({ firstName: e.target.value }); }} />
              {
                this.state.firstName.length < 1
                  ? <FormHelperText id="fn-error">Voornaam vereist</FormHelperText>
                  : null
              }
            </FormControl>
            <FormControl className={classes.margin}>
              <InputLabel htmlFor="lastNameInput">Achternaam:</InputLabel>
              <Input aria-describedby="ln-error" error={this.state.lastName.length < 1} id="lastNameInput" type="text" onChange={(e) => { this.setState({ lastName: e.target.value }); }} />
              {
                this.state.lastName.length < 1
                  ? <FormHelperText id="ln-error">Familienaam vereist</FormHelperText>
                  : null
              }
            </FormControl>
            <div className={classes.margin}>
              <FormControl className={classes.margin}>
                <InputLabel htmlFor="emailInput">E-mail:</InputLabel>
                <Input aria-describedby="em-error" error={this.state.email.length < 3 && !this.state.email.includes('@') && !this.state.email.includes('.')} id="emailInput" type="email" onChange={(e) => { this.setState({ email: e.target.value }); }} />
                {
                  this.state.email.length < 3 && !this.state.email.includes('@') && !this.state.email.includes('.')
                    ? <FormHelperText id="em-error">Email adres vereist</FormHelperText>
                    : null
                }
              </FormControl>

            </div>
            <div className={classes.margin}>

              <FormControl className={classes.margin}>
                
                <InputLabel htmlFor="passwordInput">Tijdelijk wachtwoord:</InputLabel>
                <Input aria-describedby="component-error-passwd-length" error={this.state.password.length < 6} id="passwordInput" type="password" onChange={(e) => { this.setState({ password: e.target.value }); }} />
                {
                  this.state.password.length < 6 ?
                    <FormHelperText id="component-error-passwd-length">Minstens 6 tekens</FormHelperText>
                    : null
                }
              </FormControl>
              <FormControl className={classes.margin}>
                <InputLabel htmlFor="confirmPasswordInput">Herhaal wachtwoord:</InputLabel>
                <Input aria-describedby="component-error-passwd-same" error={this.state.password !== this.state.confirmPassword} id="confirmPasswordInput" type="password" onChange={(e) => { this.setState({ confirmPassword: e.target.value }); }} />
                {
                  this.state.password !== this.state.confirmPassword
                    ? <FormHelperText id="component-error-passwd-same">Pawoorden moeten matchen</FormHelperText>
                    : null
                }
              </FormControl>
            </div>
            <div className={classes.margin}>
              <FormControl className={classes.formControl}>
                <InputLabel htmlFor="uncontrolled-native">Rol</InputLabel>
                <NativeSelect
                  defaultValue={30}
                  inputProps={{
                    name: 'Role',
                    id: 'uncontrolled-native',
                  }}
                  onChange={(e) => { this.setState({ role: e.target.value }); }}
                >
                  <option disabled={true} aria-label="None" value="" />
                  <option value={'ADMINISTRATOR'}>Administrator</option>
                  <option value={'ACCOUNTANT'}>Accountant</option>
                  <option value={'AGENDA'}>Agendabeheerder</option>
                  <option value={'MANAGER'}>Manager</option>
                  <option value={'WEGER'}>Weger</option>
                  <option value={'NUTRITION'}>Voedingsbeheerder</option>
                  <option value={'WERKNEMER'}>Werknemer</option>
                </NativeSelect>
              </FormControl>
            </div>
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button color="primary" onClick={this.handleClose}>
            Annuleren
          </Button>
          <Button
            color="primary" onClick={this.handleSave}
            disabled={
              this.state.firstName.length < 1
              || this.state.lastName.length < 1
              || this.state.confirmPassword !== this.state.password
              || this.state.password.length < 6
              || this.state.email.length < 3 && !this.state.email.includes('@') && !this.state.email.includes('.')
              || !(['ADMINISTRATOR', 'ACCOUNTANT', 'AGENDA', 'MANAGER', 'WEGER', 'NUTRITION', 'WERKNEMER'].includes(this.state.role))
            }
          >
            Opslaan
          </Button>
        </DialogActions>
      </Dialog >
    );
  }
}

export default withStyles(tableStyle)(UserAddDialog);
