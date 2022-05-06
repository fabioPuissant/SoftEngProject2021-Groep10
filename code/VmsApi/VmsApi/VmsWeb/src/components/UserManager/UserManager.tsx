import React from "react";
import "perfect-scrollbar/css/perfect-scrollbar.css";
import withStyles from "@material-ui/core/styles/withStyles";
import { Button, Card, Fab, LinearProgress, Tooltip } from "@material-ui/core";
import Table from "@material-ui/core/Table";
import TableHead from "@material-ui/core/TableHead";
import TableRow from "@material-ui/core/TableRow";
import TableBody from "@material-ui/core/TableBody";
import TableCell from "@material-ui/core/TableCell";
import AddIcon from '@material-ui/icons/Add';
import tableStyle from "../../assets/jss/material-dashboard-react/components/tableStyle";
import { doGetApiCall, deleteUserApiCall } from "../../utils/apicommunications";
import UserAddDialog from "./UserAddDialog";
import { withHooksComponent } from '../../custom-hooks/wrapperHook';
import { Delete } from "@material-ui/icons";
import AlterRolesDialog from "./AlterRolesDialog";

interface Props {
  classes: any;
  location: any;
  userDetail: UserDetails;
  token: string;
}


interface State {
  currentUser: string | null;
  openDialog: boolean;
  openAlterRoles: boolean;
  isloading: boolean;
  changeRolesUser: UserPassing;
  usersData:
  | {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    roles: Array<string>;
  }[]
  | null;
}

class UserManager extends React.Component<Props, State> {
  userDetail: UserDetails;
  token: string;
  refs: any;
  allowed: boolean;
  constructor(props: Props) {
    super(props);
    this.userDetail = props.userDetail;
    this.token = props.token;
    this.allowed = this.userDetail.roles.includes('Administrator') || this.userDetail.roles.includes('Manager');
    this.state = {
      changeRolesUser: { firstname: '', lastname: '', email: '', roles: [], token: '', id: '' },

      currentUser: null,
      openDialog: false,
      openAlterRoles: false,
      isloading: true,
      usersData: [],
    };
  }

  deleteUser(userid: string) {
    deleteUserApiCall(`Accounts/Delete/${userid}`, this.userDetail.token).then(e => {
      this.getUsers();
    })
      .catch(e => console.log('Error while deleting', e));
  }

  openDialog(): boolean {
    return this.state.currentUser ? true : false;
  }

  async getUsers(): Promise<void> {
    await doGetApiCall("accounts",  this.userDetail.token, (data: any) => {
      this.setState({ usersData: data, isloading: false });
    });
  }

  handleClose = () => {
    this.setState({ currentUser: null });
  };

  addUser = (id: string, firstName: string, lastName: string, email: string, role: string) => {
    this.getUsers();
  }
  componentDidMount() {
    this.getUsers();
  }



  customTable() {
    const tableHeaderColor = "primary";
    const tableHead = [
      "Voornaam",
      "Familie naam",
      "E-mail",
      "Rollen"
    ];
    const { classes } = this.props;
    return (
      <>
        {this.state.isloading === false ? (
          <div className={classes.tableResponsive}>
            {
              this.allowed ?
                <Button onClick={() => this.setState({ openDialog: true })} variant="outlined" color="primary"><AddIcon />Toevoegen</Button>
                : <></>
            }
            <Table className={classes.table}>
              <TableHead className={classes[tableHeaderColor + "TableHeader"]}>
                <TableRow>
                  {tableHead.map((prop: any, key: any) => {
                    return (
                      <TableCell
                        className={classes.tableCell + ' ' + classes.tableHeadCell}
                        key={key}
                      >
                        {prop}
                      </TableCell>
                    );
                  })}
                  {
                    this.allowed
                      ? (
                        <>
                          <TableCell></TableCell>
                          <TableCell></TableCell>
                        </>
                      )
                      : null
                  }
                </TableRow>
              </TableHead>
              <TableBody>
                {this.state.usersData!.map((prop, key) => {
                  return (
                    <TableRow
                      key={key}
                    >
                      <TableCell className={classes.tableCell}>
                        {prop.firstName}
                      </TableCell>
                      <TableCell className={classes.tableCell}>
                        {prop.lastName}
                      </TableCell>
                      <TableCell className={classes.tableCell}>
                        {prop.email}
                      </TableCell>
                      <TableCell className={classes.tableCell}>
                        {prop.roles.join(', ')}
                      </TableCell>
                      {

                        this.allowed && prop.id !== this.userDetail.userref && !prop.roles.includes("Administrator")
                          ? (
                            <TableCell className={classes.tableCell}>
                              <Button variant="outlined" color="default" onClick={() =>
                                this.setState(
                                  {
                                    changeRolesUser: {
                                      firstname: prop.firstName,
                                      lastname: prop.lastName,
                                      id: prop.id,
                                      token: this.userDetail.token,
                                      email: prop.email,
                                      roles: prop.roles
                                    },
                                    openAlterRoles: true
                                  })}
                              >Beheer rollen</Button>
                            </TableCell>
                          )
                          : <TableCell className={classes.tableCell}/>
                      }
                      {
                        this.allowed && prop.id !== this.userDetail.userref && !prop.roles.includes("Administrator")
                          ? (
                            <TableCell className={classes.tableCell}>
                              <Button variant="outlined" color="secondary" onClick={() => this.deleteUser(prop.id)} > <Delete /> Verwijder</Button>
                            </TableCell>
                          )
                          : <TableCell className={classes.tableCell}/>
                      }

                    </TableRow>
                  );
                })}
              </TableBody>
            </Table>

            <UserAddDialog
              open={this.state.openDialog}
              onClose={() => this.setState({ openDialog: false })}
              userAdd={this.addUser}
              token={this.userDetail.token}
            />

            {
              this.state.openAlterRoles
                // tslint:disable-next-line: jsx-wrap-multiline
                ? <AlterRolesDialog
                  onClose={() => this.setState({ ...this.state, openAlterRoles: false })}
                  open={this.state.openAlterRoles}
                  onSuccess={() => {
                    this.getUsers();
                    this.setState({ ...this.state, openAlterRoles: false });
                  }}
                  user={this.state.changeRolesUser}
                />
                : <></>
            }
          </div>
        ) : (
          <LinearProgress />
        )
        }
      </>
    );
  }

  render() {
    const { classes, ...rest } = this.props;
    let groupUis: JSX.Element[] = [];
    return this.customTable();
  }
}

export default withHooksComponent(withStyles(tableStyle)(UserManager));