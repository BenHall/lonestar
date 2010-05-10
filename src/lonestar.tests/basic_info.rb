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
    	puts step_match.format_args(lambda{|param| "#{param}"})
		puts status
		if(status == :failed || status == :pending)
		  puts("#{exception.message} | (#{exception.class}) | #{exception.backtrace.join("|")}".indent(2))
		end
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