﻿using FluentValidation;
using FluentValidation.Results;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;

namespace NetLib.WPF
{
    [PublicAPI]
    public class ValidationTemplate : INotifyDataErrorInfo
    {
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IValidator> validators =
            new ConcurrentDictionary<RuntimeTypeHandle, IValidator>();

        private readonly INotifyPropertyChanged target;
        private readonly IValidator validator;
        private ValidationResult validationResult;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => validationResult.Errors.Count > 0;

        public ValidationTemplate([NotNull] INotifyPropertyChanged target)
        {
            this.target = target;
            validator = GetValidator(target.GetType());
            validationResult = validator.Validate(target);
            target.PropertyChanged += Validate;
        }

        public static IValidator GetValidator([NotNull] Type modelType)
        {
            if (validators.TryGetValue(modelType.TypeHandle, out IValidator validator)) return validator;
            var typeName = $"{modelType.Namespace}.{modelType.Name}Validator";
            var type = modelType.Assembly.GetType(typeName, true);
            validators[modelType.TypeHandle] = validator = (IValidator)Activator.CreateInstance(type);
            return validator;
        }

        [NotNull]
        public IEnumerable GetErrors(string propertyName)
        {
            return validationResult.Errors
                .Where(x => x.PropertyName == propertyName)
                .Select(x => x.ErrorMessage);
        }

        private void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private void Validate(object sender, PropertyChangedEventArgs e)
        {
            validationResult = validator.Validate(target);
            foreach (var error in validationResult.Errors)
            {
                RaiseErrorsChanged(error.PropertyName);
            }
        }
    }
}