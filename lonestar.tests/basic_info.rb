module Meerkatalyst 
  module Lonestar
    class BasicInfo
      def initialize(step_mother, io, options)
      end

      def feature_name(name)
        puts "feature_name"
		puts name
      end

      def scenario_name(keyword, name, file_colon_line, source_indent)
        puts "scenario_name"
        puts name
      end

      def after_step_result(keyword, step_match, multiline_arg, status, exception, source_indent, background)
        puts "after_step_result"
        puts step_match.instance_variable_get("@name")
		puts status.to_s
      end

	  def after_feature(feature)
        puts "feature_done"
      end

	  def after_steps(steps)
	  	puts "steps_done"
	  end
    end
  end
end