import React from 'react';
import classNames from 'classnames';
// @material-ui/core components
import withStyles from '@material-ui/core/styles/withStyles';
import FormControl from '@material-ui/core/FormControl';
import InputLabel from '@material-ui/core/InputLabel';
import Input from '@material-ui/core/Input';
// @material-ui/icons
import Clear from '@material-ui/icons/Clear';
import Check from '@material-ui/icons/Check';
// core components
import customInputStyle from '../../assets/jss/material-dashboard-react/components/customInputStyle';

function CustomInput({ ...props }: any) {
  const {
    classes,
    formControlProps,
    labelText,
    id,
    labelProps,
    inputProps,
    error,
    success,
    enabled,
    inputVal,
    changeCallback,
    passwd
  } = props;

  const labelClasses = classNames({
    [' ' + classes.labelRootError]: error,
    [' ' + classes.labelRootSuccess]: success && !error
  });

  const underlineClasses = classNames({
    [classes.underlineError]: error,
    [classes.underlineSuccess]: success && !error,
    [classes.underline]: true
  });

  const marginTop = classNames({
    [classes.marginTop]: labelText === undefined
  });

  return (
    <FormControl
      {...formControlProps}
      className={formControlProps.className + ' ' + classes.formControl}
    >
      {labelText !== undefined ? (
        <InputLabel
          className={classes.labelRoot + labelClasses}
          htmlFor={id}
          {...labelProps}
        >
          {labelText}
        </InputLabel>
      ) : null}
      <Input
        type={passwd ? 'password' : 'text'}
        disabled={!enabled}
        style={{ border: 'none' }}
        value={inputVal}
        onChange={(e: any) => changeCallback(e.target.value)}
        classes={{
          root: marginTop,
          disabled: classes.disabled,
          //underline: underlineClasses
        }}
        id={id}
        {...inputProps}
      />
      {error ? (
        <Clear className={classes.feedback + ' ' + classes.labelRootError} />
      ) : success ? (
        <Check className={classes.feedback + ' ' + classes.labelRootSuccess} />
      ) : null}
    </FormControl>
  );
}

// CustomInput.propTypes = {
//   classes: PropTypes.object.isRequired,
//   labelText: PropTypes.node,
//   labelProps: PropTypes.object,
//   id: PropTypes.string,
//   inputProps: PropTypes.object,
//   formControlProps: PropTypes.object,
//   error: PropTypes.bool,
//   success: PropTypes.bool
// };

export default withStyles(customInputStyle)(CustomInput);
