import {
  warningColor,
  primaryColor,
  dangerColor,
  successColor,
  infoColor,
  roseColor,
  grayColor,
  defaultFont
} from '../../material-dashboard-react';
import { createStyles, Theme } from '@material-ui/core';

const tableStyle = (theme: Theme) => createStyles({
  warningTableHeader: {
    color: warningColor[0]
  },
  primaryTableHeader: {
    color: primaryColor[0]
  },
  dangerTableHeader: {
    color: dangerColor[0]
  },
  successTableHeader: {
    color: successColor[0]
  },
  infoTableHeader: {
    color: infoColor[0]
  },
  roseTableHeader: {
    color: roseColor[0]
  },
  grayTableHeader: {
    color: grayColor[0]
  },
  table: {
    marginBottom: '0',
    width: '100%',
    maxWidth: '100%',
    backgroundColor: 'transparent',
    borderSpacing: '0',
    borderCollapse: 'collapse'
  },
  tableHeadCell: {
    color: 'inherit',
    ...defaultFont,
    fontSize: '1em'
  },
  tableCell: {
    ...defaultFont,
    lineHeight: '1.42857143',
    padding: '12px 8px',
    verticalAlign: 'middle'
  },
  tableResponsive: {
    width: '100%',
    marginTop: theme.spacing.unit * 3,
    overflowX: 'auto'
  },
  modal: {
    fontSize: "12px"
  },
  modalHeader: {
    width: "100%",
    borderBottom: "1px solid gray",
    fontSize: "18px",
    textAlign: "center",
    padding: "5px"
  },
  modalContent: {
    fontSize: "12px",
    width: "100%",
    padding: "10px 5px",
  },
  modalActions: {
    fontSize: "12px",
    width: "100%",
    padding: "10px 5px",
    margin: "auto",
    textAlign: "center"
  },
  modalClose: {
    cursor: "pointer",
    position: "absolute",
    display: "block",
    padding: "2px 5px",
    lineHeight: "20px",
    right: "-10px",
    top: "-10px",
    fontSize: "24px",
    background: "#ffffff",
    borderRadius: "18px",
    border: "1px solid #cfcece"
  },
  contentStyle: {
    width:"600px"
  },
  // custom cells
  root: {
    width: "100%",
    marginTop: theme.spacing.unit * 3,
    overflowX: "auto"
  },
  selectTableCell: {
    width: "60 px"
  },
  input: {
    width: "130 px",
    height: "40 px"
  },
  margin: {
    marginTop: "10px"
  }
  
});

export default tableStyle;
