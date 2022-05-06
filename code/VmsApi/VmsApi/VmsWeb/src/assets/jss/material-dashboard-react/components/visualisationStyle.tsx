import {
    defaultFont,
    primaryColor,
    dangerColor,
    grayColor
  } from '../../material-dashboard-react';
  import tooltipStyle from '../tooltipStyle';
  import checkboxAdnRadioStyle from '../checkboxAdnRadioStyle';
  import { createStyles } from '@material-ui/core';

  const visualisationStyle = createStyles({
    multiSelect: {
      width: '20%',
      display: 'inline-block'
    },
    searchButton: {
      display: 'inline-block',
      marginBottom: '7px',
      marginLeft: '20px'
    },
  });
  export default visualisationStyle;