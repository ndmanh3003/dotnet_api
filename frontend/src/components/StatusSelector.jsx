import {Dropdown, Tag} from 'antd';

const StatusSelector = ({value, onChange, statusOptions, getStatusColor, placeholder = 'Chọn trạng thái', closable = false}) => {
  return (
      <Dropdown
          menu={{
            items: statusOptions.map(opt => ({
              key: opt.value,
              label: opt.label,
              onClick: () => onChange(opt.value),
            })),
          }}
          trigger={['click']}
      >
        {value ? (
            <Tag
                color={getStatusColor(value)}
                closable={closable}
                onClose={(e) => {
                  e.stopPropagation();
                  onChange(null);
                }}
                style={{borderRadius: '999px', padding: '8px 16px', margin: 0, cursor: 'pointer', height: '40px', display: 'inline-flex', alignItems: 'center', fontSize: '16px'}}
            >
              {statusOptions.find(opt => opt.value === value)?.label || value}
            </Tag>
        ) : (
            <Tag
                style={{borderRadius: '999px', padding: '8px 16px', margin: 0, cursor: 'pointer', borderStyle: 'dashed', height: '40px', display: 'inline-flex', alignItems: 'center', fontSize: '16px'}}
            >
              {placeholder}
            </Tag>
        )}
      </Dropdown>
  );
};

export default StatusSelector;

