/* eslint-disable */
import React from "react";
import "perfect-scrollbar/css/perfect-scrollbar.css";
// @material-ui/core components
import withStyles from "@material-ui/core/styles/withStyles";

import Table from "@material-ui/core/Table";
import TableHead from "@material-ui/core/TableHead";
import TableRow from "@material-ui/core/TableRow";
import TableBody from "@material-ui/core/TableBody";
import TableCell from "@material-ui/core/TableCell";
// core components
import tableStyle from "../../assets/jss/material-dashboard-react/components/tableStyle";
import { IconButton, Input } from "@material-ui/core";
import EditIcon from "@material-ui/icons/EditOutlined";
import DoneIcon from "@material-ui/icons/DoneAllTwoTone";
import RevertIcon from "@material-ui/icons/NotInterestedOutlined";
import DeleteIcon from "@material-ui/icons/Delete";
import { stringifyConfiguration } from "tslint/lib/configuration";
import moment from "moment";
import { isNamedExportBindings } from "typescript";

import { withHooksComponent } from "../../custom-hooks/wrapperHook";
import { checkRole } from "../../utils/RoleChecker";
// core components

interface Props {
  userDetail: UserDetails;
  classes: any;
  birthDate: Date;
  measurePoints: {
    id: string;
    date: Date;
    weight: number;
    amount: number;
    changed: boolean;
  }[];
  onChange(
    measurePoints: {
      id: string;
      date: Date;
      weight: number;
      amount: number;
      changed: boolean;
      deleted: boolean;
    }[]
  ): void;
}

interface State {
  previous: any;
  rows: rowInterface[];
}

interface rowInterface {
  id: string;
  date: Date;
  amount: number;
  weight: number;
  avgWeight: string;
  isEditMode: boolean;
  changed: boolean;
  deleted: boolean;
}
class GroupWeightTable extends React.Component<Props, State> {
  constructor(props: Props) {
    super(props);
    this.state = {
      previous: {},
      rows: [],
    };
  }

  componentDidMount() {
    this.setState({
      rows: this.props.measurePoints.map((x) => this.createData(x)),
    });
  }

  createData = ({
    id,
    date,
    weight,
    amount,
  }: {
    id: string;
    date: Date;
    weight: number;
    amount: number;
  }) => ({
    id,
    date,
    amount: amount,
    weight: weight,
    avgWeight: (weight / amount).toFixed(2),
    isEditMode: false,
    changed: false,
    deleted: false,
  });

  customTableCell = ({row, name, type, }: { row: any; name: any; type: string; }) => {
    const classes = this.props.classes;
    const { isEditMode } = row;
    return (
      <TableCell align="left" className={classes.tableCell}>
        {isEditMode &&
        checkRole(
          this.props.userDetail.roles != undefined
            ? this.props.userDetail.roles
            : [],
          ["Manager", "Weger"]
        ) ? (
          <Input
            type={type}
            value={
              name === "date"
                ? moment(row[name]).format("YYYY-MM-DD")
                : row[name]
            }
            name={name}
            onChange={(e) => this.onChange(e, row)}
            className={classes.input}
          />
        ) : name !== "date" ? (
          row[name]
        ) : (
          moment(row[name]).format("DD/MM/YYYY")
        )}
      </TableCell>
    );
  };

  onToggleEditMode = (id: any) => {
    let test = [...this.state.rows];

    this.setState({
      rows: test.map((row) => {
        if (row.id === id) {
          return { ...row, isEditMode: !row.isEditMode };
        } else return row;
      }),
    });
  };

  onSaveEditMode = async (id: any) => {
    let test = [...this.state.rows];

    await this.setState({
      rows: test.map((row) => {
        if (row.id === id) {
          return { ...row, isEditMode: !row.isEditMode, changed: true };
        } else return row;
      }),
    });

    this.props.onChange(this.state.rows);
  };

  onChange = (e: any, row: any) => {
    if (!this.state.previous[row.id]) {
      let test = { ...this.state.previous };
      test[row.id] = row;
      this.setState({ previous: test });
    }
    const value = e.target.value;
    const name = e.target.name;
    const { id } = row;
    const newRows = this.state.rows.map((row) => {
      if (row.id === id) {
        return { ...row, [name]: value };
      }
      return row;
    });
    this.setState({ rows: newRows });
  };

  onRevert = async (id: any) => {
    const newRows = this.state.rows.map((row) => {
      if (row.id === id) {
        return this.state.previous[id] ? this.state.previous[id] : row;
      }else return row;
    });

    await this.setState({ rows: newRows });
    let test = { ...this.state.previous };
    delete test[id];
    this.setState({ previous: test });
    this.onToggleEditMode(id);
  };

  onDelete = async (id: any) => {
    let rows = [...this.state.rows];

    await this.setState({
      rows: rows.map((row) => {
        if (row.id === id) {
          return { ...row, deleted: true };
        } else return row;
      }),
    });

    this.props.onChange(this.state.rows);
  };

  render() {
    const { classes, ...rest } = this.props;
    let groupUis: JSX.Element[] = [];
    const tableHeaderColor = "primary";
    const tableWeightHead = [
      "Datum",
      "# Biggen",
      "Totaal gewicht (kg)",
      "Gemiddeld gewicht/big (kg)",
    ];

    return (
      <div>
        <p>Wegingen:</p>

        <Table className={classes.table}>
          <TableHead className={classes[tableHeaderColor + "TableHeader"]}>
            <TableRow>
            {checkRole(
                this.props.userDetail.roles != undefined
                  ? this.props.userDetail.roles
                  : [],
                ["Manager", "Weger"]
              ) ? <TableCell align="left" /> : null}
              {tableWeightHead.map((prop: any, key: any) => {
                return (
                  <TableCell
                    className={classes.tableCell + " " + classes.tableHeadCell}
                    key={key}
                  >
                    {prop}
                  </TableCell>
                );
              })}
              {checkRole(
                this.props.userDetail.roles != undefined
                  ? this.props.userDetail.roles
                  : [],
                ["Manager", "Weger"]
              ) && this.state.rows.length > 1 ? (
                <TableCell
                  className={classes.tableCell + " " + classes.tableHeadCell}
                  key={"delete"}
                >
                  Verwijderen
                </TableCell>
              ) : null}
            </TableRow>
          </TableHead>
          <TableBody>
            {this.state.rows
              .filter((x) => !x.deleted)
              .map((row) => {
                return (
                  <TableRow key={row.id}>
                    {checkRole(
                this.props.userDetail.roles != undefined
                  ? this.props.userDetail.roles
                  : [],
                ["Manager", "Weger"]) ?
                    <TableCell className={classes.selectTableCell}>
                      {row.isEditMode ? (
                        <>
                          <IconButton
                            aria-label="done"
                            onClick={() => this.onSaveEditMode(row.id)}
                          >
                            <DoneIcon />
                          </IconButton>
                          <IconButton
                            aria-label="revert"
                            onClick={() => this.onRevert(row.id)}
                          >
                            <RevertIcon />
                          </IconButton>
                        </>
                      ) : (
                        <IconButton
                          aria-label="delete"
                          onClick={() => this.onToggleEditMode(row.id)}
                        >
                          <EditIcon />
                        </IconButton>
                      )}
                    </TableCell>: null}

                    {this.customTableCell({ row, name: "date", type: "date" })}
                    {this.customTableCell({
                      row,
                      name: "amount",
                      type: "number",
                    })}
                    {this.customTableCell({
                      row,
                      name: "weight",
                      type: "number",
                    })}
                    <TableCell className={classes.tableCell}>
                      {Number(row.avgWeight)}
                    </TableCell>

                    {checkRole(
                      this.props.userDetail.roles != undefined
                        ? this.props.userDetail.roles
                        : [],
                      ["Manager", "Weger"]
                    ) && moment(row.date).format("DD/MM/YYYY") !== moment(this.props.birthDate).format("DD/MM/YYYY") ? (
                      <TableCell className={classes.selectTableCell}>
                        <IconButton
                          aria-label="done"
                          onClick={() => this.onDelete(row.id)}
                        >
                          <DeleteIcon />
                        </IconButton>
                      </TableCell>
                    ) : null}
                  </TableRow>
                );
              })}
          </TableBody>
        </Table>
      </div>
    );
  }
}

export default withHooksComponent(withStyles(tableStyle)(GroupWeightTable));
